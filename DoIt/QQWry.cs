using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DoIt
{

    #region 调用方法

    /*===================================================================================
     * 使用方法：
     *      //获取纯真数据库路径
     *      string dbip = HttpContext.Current.Server.MapPath(filepath);
     *      //创建对象
     *      QQWry qq = new QQWry(dbip);
     *      //产生IP地址
     *      IPLocation loca = qq.SearchIPLocation(ip);
     *      //调用如下，城市和地区
     *      loca.country|loca.area
     * 
    ===================================================================================*/
    #endregion

    #region 纯真IP库

    /// <summary> 
    /// QQWry 的摘要说明。 
    /// </summary> 
    /// class QQWry 
    public class QqWry 
    { 
        /// <summary> 
        /// 第一种模式 
        /// </summary> 
        private const byte RedirectMode1 = 0x01; 

        /// <summary> 
        /// 第二种模式 
        /// </summary> 
        private const byte RedirectMode2 = 0x02; 

        /// <summary> 
        /// 每条记录长度 
        /// </summary>        
        private const int IpRecordLength = 7; 

        /// <summary> 
        /// 文件对象 
        /// </summary>         
        private FileStream _ipFile; 
        private const string UnCountry = "未知国家"; 
        private const string UnArea    = "未知地区"; 

        /// <summary> 
        /// 索引开始位置 
        /// </summary> 
        private long _ipBegin; 

        /// <summary> 
        /// 索引结束位置 
        /// </summary> 
        private long _ipEnd; 

        /// <summary> 
        /// IP对象 
        /// </summary> 
        private IpLocation _loc; 

        /// <summary> 
        /// 存储文本内容 
        /// </summary> 
        private byte[] _buf; 

        /// <summary> 
        /// 存储3字节 
        /// </summary> 
        private byte[] _b3; 

        /// <summary> 
        /// 存储4字节IP地址 
        /// </summary> 
        private byte[] _b4; 

        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="ipfile">IP数据库文件绝对路径</param> 
        public QqWry( string ipfile ) 
        {             
            _buf = new byte[100]; 
            _b3 = new byte[3]; 
            _b4 = new byte[4];             
            try 
            { 
                _ipFile = new FileStream(ipfile,FileMode.Open,FileAccess.Read,FileShare.Read); 
            } 
            catch( Exception ex ) 
            { 
                throw new Exception( ex.Message ); 
            }             
            _ipBegin = ReadLong4(0); 
            _ipEnd = ReadLong4(4); 
            _loc = new IpLocation();
        } 

        /// <summary> 
        /// 搜索IP地址搜索 
        /// </summary> 
        /// <param name="ip"></param> 
        /// <returns></returns> 
        public IpLocation SearchIpLocation( string ip ) 
            { 
                //将字符IP转换为字节 
                string[] ipSp = ip.Split('.'); 
                if( ipSp.Length != 4 ) 
                { 
                    throw new ArgumentOutOfRangeException( "不是合法的IP地址!" ); 
                } 
                byte[] IP = new byte[4]; 
                for( int i = 0; i < IP.Length ; i++ ) 
                { 
                    IP[i] = (byte)(Int32.Parse( ipSp[i] ) & 0xFF) ; 
                } 
                IpLocation local = null; 
                long offset = LocateIp( IP ); 
                if( offset != -1 ) 
                { 
                    local = GetIpLocation( offset ); 
                } 
                if( local == null ) 
                { 
                    local = new IpLocation(); 
                    local.Area = UnArea; 
                    local.Country = UnCountry; 
                } 
                return local; 
            } 

        /// <summary> 
        /// 取得具体信息 
        /// </summary> 
        /// <param name="offset"></param> 
        /// <returns></returns> 
        private IpLocation GetIpLocation( long offset ) 
        { 
            _ipFile.Position = offset + 4; 
            //读取第一个字节判断是否是标志字节 
            byte one = (byte)_ipFile.ReadByte(); 
            if( one == RedirectMode1 ) 
            { 
                //第一种模式 
                //读取国家偏移 
                long countryOffset = readLong3(); 
                //转至偏移处 
                _ipFile.Position = countryOffset; 
                //再次检查标志字节 
                byte b = (byte)_ipFile.ReadByte(); 
                if( b == RedirectMode2 ) 
                { 
                    _loc.Country = ReadString( readLong3() ); 
                    _ipFile.Position = countryOffset + 4; 
                } 
                else 
                    _loc.Country = ReadString( countryOffset ); 
                //读取地区标志 
                _loc.Area = ReadArea( _ipFile.Position ); 
            } 
            else if( one == RedirectMode2 ) 
            { 
                //第二种模式 
                _loc.Country = ReadString( readLong3() ); 
                _loc.Area = ReadArea( offset + 8 ); 
            } 
            else 
            { 
                //普通模式 
                _loc.Country = ReadString( --_ipFile.Position ); 
                _loc.Area = ReadString( _ipFile.Position ); 
            }
            return _loc; 
        } 

        /// <summary> 
        /// 读取地区名称 
        /// </summary> 
        /// <param name="offset"></param> 
        /// <returns></returns> 
        private string ReadArea( long offset ) 
        { 
            _ipFile.Position = offset; 
            byte one = (byte)_ipFile.ReadByte(); 
            if( one == RedirectMode1 || one == RedirectMode2 ) 
            { 
                long areaOffset = readLong3( offset + 1 ); 
                if( areaOffset == 0 ) 
                    return UnArea; 
                else 
                { 
                    return ReadString( areaOffset ); 
                } 
            } 
            else 
            { 
                return ReadString( offset ); 
            } 
        } 

        /// <summary> 
        /// 读取字符串 
        /// </summary> 
        /// <param name="offset"></param> 
        /// <returns></returns> 
        private string ReadString( long offset ) 
        { 
            _ipFile.Position = offset; 
            int i = 0; 
            for(i = 0, _buf[i]=(byte)_ipFile.ReadByte();_buf[i] != (byte)(0);_buf[++i]=(byte)_ipFile.ReadByte()); 
            if( i > 0 ) 
                return Encoding.Default.GetString( _buf,0,i ); 
            else 
                return ""; 
        } 

        /// <summary> 
        /// 查找IP地址所在的绝对偏移量 
        /// </summary> 
        /// <param name="ip"></param> 
        /// <returns></returns> 
        private long LocateIp( byte[] ip ) 
        { 
            long m = 0; 
            int r; 
            //比较第一个IP项 
            ReadIp( _ipBegin, _b4 ); 
            r = CompareIp( ip,_b4); 
            if( r == 0 ) 
                return _ipBegin; 
            else if( r < 0 ) 
                return -1; 
            //开始二分搜索 
            for( long i = _ipBegin,j=_ipEnd; i<j; ) 
            { 
                m = this.GetMiddleOffset( i,j ); 
                ReadIp( m,_b4 ); 
                r = CompareIp( ip,_b4 ); 
                if( r > 0 ) 
                    i = m; 
                else if( r < 0 ) 
                { 
                    if( m == j ) 
                    { 
                        j -= IpRecordLength; 
                        m = j; 
                    } 
                    else 
                    { 
                        j = m; 
                    } 
                } 
                else 
                    return readLong3( m+4 ); 
            } 
            m = readLong3( m+4 ); 
            ReadIp( m,_b4 ); 
            r = CompareIp( ip,_b4 ); 
            if( r <= 0 ) 
                return m; 
            else 
                return -1; 
        } 

        /// <summary> 
        /// 从当前位置读取四字节,此四字节是IP地址 
        /// </summary> 
        /// <param name="offset"></param> 
        /// <param name="ip"></param> 
        private void ReadIp( long offset, byte[] ip ) 
        { 
            _ipFile.Position = offset; 
            _ipFile.Read( ip,0,ip.Length ); 
            byte tmp = ip[0]; 
            ip[0] = ip[3]; 
            ip[3] = tmp; 
            tmp = ip[1]; 
            ip[1] = ip[2]; 
            ip[2] = tmp; 
        } 

        /// <summary> 
        /// 比较IP地址是否相同 
        /// </summary> 
        /// <param name="ip"></param> 
        /// <param name="beginIp"></param> 
        /// <returns>0:相等,1:ip大于beginIP,-1:小于</returns> 
        private int CompareIp( byte[] ip, byte[] beginIp ) 
        { 
            for( int i = 0; i < 4; i++ ) 
            { 
                int r = CompareByte( ip[i],beginIp[i] ); 
                if( r != 0 ) 
                    return r; 
            } 
            return 0; 
        } 

        /// <summary> 
        /// 比较两个字节是否相等 
        /// </summary> 
        /// <param name="bsrc"></param> 
        /// <param name="bdst"></param> 
        /// <returns></returns> 
        private int CompareByte( byte bsrc, byte bdst ) 
        { 
            if( ( bsrc&0xFF ) > ( bdst&0xFF ) ) 
                return 1; 
            else if( (bsrc ^ bdst) == 0 ) 
                return 0; 
            else 
                return -1; 
        } 

        /// <summary> 
        /// 从当前位置读取4字节,转换为长整型 
        /// </summary> 
        /// <param name="offset"></param> 
        /// <returns></returns> 
        private long ReadLong4( long offset ) 
        { 
            long ret = 0; 
            _ipFile.Position = offset; 
            ret |= ( (long)_ipFile.ReadByte() & 0xFF ); 
            ret |= ( (long)( _ipFile.ReadByte() << 8 ) & 0xFF00 ); 
            ret |= ( (long)( _ipFile.ReadByte() << 16 ) & 0xFF0000 ); 
            ret |= ( (long)( _ipFile.ReadByte() << 24 ) & 0xFF000000 ); 
            return ret; 
        } 

       /// <summary> 
       /// 根据当前位置,读取3字节 
       /// </summary> 
       /// <param name="offset"></param> 
       /// <returns></returns> 
       private long readLong3( long offset ) 
       { 
           long ret = 0; 
           _ipFile.Position = offset; 
           ret |= ( (long)_ipFile.ReadByte() & 0xFF ); 
           ret |= ( (long)(_ipFile.ReadByte() << 8 ) & 0xFF00 ); 
           ret |= ( (long)(_ipFile.ReadByte() << 16 ) & 0xFF0000 ); 
           return ret; 
       } 
 
        /// <summary> 
        /// 从当前位置读取3字节 
        /// </summary> 
        /// <returns></returns> 
        private long readLong3() 
        { 
            long ret = 0;             
            ret |= ( (long)_ipFile.ReadByte() & 0xFF ); 
            ret |= ( (long)(_ipFile.ReadByte() << 8 ) & 0xFF00 ); 
            ret |= ( (long)(_ipFile.ReadByte() << 16 ) & 0xFF0000 ); 
            return ret; 
        } 

        /// <summary> 
        /// 取得begin和end中间的偏移 
        /// </summary> 
        /// <param name="begin"></param> 
        /// <param name="end"></param> 
        /// <returns></returns> 
        private long GetMiddleOffset( long begin, long end ) 
        { 
            long records = ( end - begin ) / IpRecordLength; 
            records >>= 1; 
            if( records == 0 ) 
                records = 1; 
            return begin + records * IpRecordLength; 
        }
    }

    #endregion

    #region IP地区实体类

    /// <summary>
    /// IP地区
    /// </summary>
    public class IpLocation
    {
        /// <summary>
        /// 城镇
        /// </summary>
        public String Country;
        /// <summary>
        /// 地区
        /// </summary>
        public String Area;
        /// <summary>
        /// IP地址
        /// </summary>
        public IpLocation()
        {
            Country = Area = "";
        }
        /// <summary>
        /// 获取版本
        /// </summary>
        /// <returns></returns>
        public IpLocation GetCopy()
        {
            IpLocation ret = new IpLocation();
            ret.Country = Country;
            ret.Area = Area;
            return ret;
        }
    }

    #endregion

}
