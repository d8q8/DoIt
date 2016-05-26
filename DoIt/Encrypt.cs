using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DoIt
{

    #region DES 加密类
    /// <summary>
    /// 此处定义的是DES加密,为了便于今后的管理和维护
    /// 请不要随便改动密码,或者改变了密码后请一定要
    /// 牢记先前的密码,否则将会照成不可预料的损失
    /// </summary>
    public class DesEncrypt
    {
        #region DES 初始化值
        private const string Iv = "8802667";
        private readonly DES _des;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public DesEncrypt()
        {
            _des = new DESCryptoServiceProvider();
        }

        #region DES 属性
        /// <summary>
        /// 设置加密密钥
        /// </summary>
        private string EncryptKey { get; set; } = "8802667";

        /// <summary>
        /// 要加密字符的编码模式
        /// </summary>
        private Encoding EncodingMode { get; set; } = new UnicodeEncoding();

        #endregion

        #region DES 方法

        #region 加解密字符串并返回结果
        /// <summary>
        /// 加密字符串并返回加密后的结果
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string EncryptString(string str)
        {
            var ivb = Encoding.ASCII.GetBytes(Iv);
            var keyb = Encoding.ASCII.GetBytes(EncryptKey);//得到加密密钥
            var toEncrypt = EncodingMode.GetBytes(str);//得到要加密的内容
            var encryptor = _des.CreateEncryptor(keyb, ivb);
            var msEncrypt = new MemoryStream();
            var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
            csEncrypt.FlushFinalBlock();
            var encrypted = msEncrypt.ToArray();
            csEncrypt.Close();
            msEncrypt.Close();
            return EncodingMode.GetString(encrypted);
        }
        /// <summary>
        /// 解密给定的字符串
        /// </summary>
        /// <param name="str">要解密的字符</param>
        /// <returns></returns>
        public string DecryptString(string str)
        {
            var ivb = Encoding.ASCII.GetBytes(Iv);
            var keyb = Encoding.ASCII.GetBytes(EncryptKey);
            var toDecrypt = EncodingMode.GetBytes(str);
            var deCrypted = new byte[toDecrypt.Length];
            var deCryptor = _des.CreateDecryptor(keyb, ivb);
            var msDecrypt = new MemoryStream(toDecrypt);
            var csDecrypt = new CryptoStream(msDecrypt, deCryptor, CryptoStreamMode.Read);
            try
            {
                csDecrypt.Read(deCrypted, 0, deCrypted.Length);
            }
            catch (Exception err)
            {
                throw new ApplicationException(err.Message);
            }
            finally
            {
                try
                {
                    msDecrypt.Close();
                    csDecrypt.Close();
                }
                catch
                {
                    // ignored
                }
            }
            return EncodingMode.GetString(deCrypted);
        }
        #endregion

        #region 加解密指定的文件
        /// <summary>
        /// 加密指定的文件,如果成功返回True,否则false
        /// </summary>
        /// <param name="filePath">要加密的文件路径</param>
        /// <param name="outPath">加密后的文件输出路径</param>
        private void EncryptFile(string filePath, string outPath)
        {
            var isExist = File.Exists(filePath);
            if (isExist)//如果存在
            {
                var ivb = Encoding.ASCII.GetBytes(Iv);
                var keyb = Encoding.ASCII.GetBytes(EncryptKey);
                //得到要加密文件的字节流
                var fin = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var reader = new StreamReader(fin, EncodingMode);
                var dataStr = reader.ReadToEnd();
                var toEncrypt = EncodingMode.GetBytes(dataStr);
                fin.Close();

                var fout = new FileStream(outPath, FileMode.Create, FileAccess.Write);
                var encryptor = _des.CreateEncryptor(keyb, ivb);
                var csEncrypt = new CryptoStream(fout, encryptor, CryptoStreamMode.Write);
                try
                {
                    //加密得到的文件字节流
                    csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
                    csEncrypt.FlushFinalBlock();
                }
                catch (Exception err)
                {
                    throw new ApplicationException(err.Message);
                }
                finally
                {
                    try
                    {
                        fout.Close();
                        csEncrypt.Close();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("没有找到指定的文件");
            }
        }
        /// <summary>
        /// 文件加密函数的重载版本,如果不指定输出路径,
        /// 那么原来的文件将被加密后的文件覆盖
        /// </summary>
        /// <param name="filePath"></param>
        public void EncryptFile(string filePath)
        {
            EncryptFile(filePath, filePath);
        }

        /// <summary>
        /// 解密指定的文件
        /// </summary>
        /// <param name="filePath">要解密的文件路径</param>
        /// <param name="outPath">解密后的文件输出路径</param>
        private void DecryptFile(string filePath, string outPath)
        {
            var isExist = File.Exists(filePath);
            if (isExist)//如果存在
            {
                var ivb = Encoding.ASCII.GetBytes(Iv);
                var keyb = Encoding.ASCII.GetBytes(EncryptKey);
                var file = new FileInfo(filePath);
                var deCrypted = new byte[file.Length];
                //得到要解密文件的字节流
                var fin = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                //解密文件
                try
                {
                    var decryptor = _des.CreateDecryptor(keyb, ivb);
                    var csDecrypt = new CryptoStream(fin, decryptor, CryptoStreamMode.Read);
                    csDecrypt.Read(deCrypted, 0, deCrypted.Length);
                }
                catch (Exception err)
                {
                    throw new ApplicationException(err.Message);
                }
                finally
                {
                    try
                    {
                        fin.Close();
                    }
                    catch
                    {
                        // ignored
                    }
                }
                var fout = new FileStream(outPath, FileMode.Create, FileAccess.Write);
                fout.Write(deCrypted, 0, deCrypted.Length);
                fout.Close();
            }
            else
            {
                throw new FileNotFoundException("指定的解密文件没有找到");
            }
        }
        /// <summary>
        /// 解密文件的重载版本,如果没有给出解密后文件的输出路径,
        /// 则解密后的文件将覆盖先前的文件
        /// </summary>
        /// <param name="filePath"></param>
        public void DecryptFile(string filePath)
        {
            DecryptFile(filePath, filePath);
        }
        #endregion
        
        #endregion

    }
    #endregion

    #region MD5 加密类
    /// <summary>
    /// MD5加密类,注意经MD5加密过的信息是不能转换回原始数据的
    /// ,请不要在用户敏感的信息中使用此加密技术,比如用户的密码,
    /// 请尽量使用对称加密
    /// </summary>
    public class Md5Encrypt
    {
        private readonly MD5 _md5;
        public Md5Encrypt()
        {
            _md5 = new MD5CryptoServiceProvider();
        }
        /// <summary>
        /// 从字符串中获取散列值
        /// </summary>
        /// <param name="str">要计算散列值的字符串</param>
        /// <returns></returns>
        public string GetMd5FromString(string str)
        {
            var toCompute = Encoding.Unicode.GetBytes(str);
            var hashed = _md5.ComputeHash(toCompute, 0, toCompute.Length);
            return Encoding.ASCII.GetString(hashed);
        }
        /// <summary>
        /// 根据文件来计算散列值
        /// </summary>
        /// <param name="filePath">要计算散列值的文件路径</param>
        /// <returns></returns>
        public string GetMd5FromFile(string filePath)
        {
            var isExist = File.Exists(filePath);
            if (isExist)//如果文件存在
            {
                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var reader = new StreamReader(stream, Encoding.Unicode);
                var str = reader.ReadToEnd();
                var toHash = Encoding.Unicode.GetBytes(str);
                var hashed = _md5.ComputeHash(toHash, 0, toHash.Length);
                stream.Close();
                return Encoding.ASCII.GetString(hashed);
            }
            else
            {
                throw new FileNotFoundException("指定的文件没有找到");
            }
        }
    }
    #endregion

    #region 数字签名 加密类
    /// <summary>
    /// 用于数字签名的hash类
    /// </summary>
    public class MacTripleDesEncrypt
    {
        private readonly MACTripleDES _mact;
        private string _key = "8802667";

        public MacTripleDesEncrypt()
        {
            _mact = new MACTripleDES();
        }
        /// <summary>
        /// 获取或设置用于数字签名的密钥
        /// </summary>
        private string Key
        {
            get { return _key; }
            set
            {
                var keyLength = value.Length;
                var keyAllowLengths = new[] { 8, 16, 24 };
                var isRight = keyAllowLengths.Any(i => keyLength == keyAllowLengths[i]);
                if (!isRight)
                    throw new ApplicationException("用于数字签名的密钥长度必须是8,16,24值之一");
                else
                    _key = value;
            }
        }
        /// <summary>
        /// 获取或设置用于数字签名的用户数据
        /// </summary>
        private byte[] Data { get; set; } = null;

        /// <summary>
        /// 得到签名后的hash值
        /// </summary>
        /// <returns></returns>
        public string GetHashValue()
        {
            if (Data == null)
                throw new ApplicationException("没有设置要进行数字签名的用户 数据(property:Data)");
            var key = Encoding.ASCII.GetBytes(Key);
            _mact.Key = key;
            var hashB = _mact.ComputeHash(_mact.ComputeHash(Data));
            return Encoding.ASCII.GetString(hashB);
        }
    }
    #endregion

    #region BASE64 加密类
    public class SBase64
    {
        #region 字符串使用base64算法加密
        /// <summary>
        /// 将字符串使用base64算法加密
        /// </summary>
        /// <param name="sourceString">待加密的字符串</param>
        /// <param name="ens">System.Text.Encoding 对象，如创建中文编码集对象：System.Text.Encoding.GetEncoding(54936)</param>
        /// <returns>加码后的文本字符串</returns>
        private static string EncodingForString(string sourceString, Encoding ens)
        {
            try
            {
                return Convert.ToBase64String(ens.GetBytes(sourceString));
            }
            catch
            {
                return sourceString;
            }
        }

        /// <summary>
        /// 将字符串使用base64算法加密
        /// </summary>
        /// <param name="sourceString">待加密的字符串</param>
        /// <returns>加码后的文本字符串</returns>
        public static string EncodingForString(string sourceString)
        {
            try
            {
                return EncodingForString(sourceString, Encoding.GetEncoding(54936));
            }
            catch
            {
                return sourceString;
            }
        }

        /// <summary>
        /// 从base64编码的字符串中还原字符串，支持中文
        /// </summary>
        /// <param name="base64String">base64加密后的字符串</param>
        /// <param name="ens">System.Text.Encoding 对象，如创建中文编码集对象：System.Text.Encoding.GetEncoding(54936)</param>
        /// <returns>还原后的文本字符串</returns>
        private static string DecodingForString(string base64String, Encoding ens)
        {
            /**
            * ***********************************************************
            * 
            * 从base64String中取得的字节值为字符的机内码（ansi字符编码）
            * 一般的，用机内码转换为汉字是公式：
            * (char)(第一字节的二进制值*256+第二字节值)
            * 而在c#中的char或string由于采用了unicode编码，就不能按照上面的公式计算了
            * ansi的字节编和unicode编码不兼容
            * 故利用.net类库提供的编码类实现从ansi编码到unicode代码的转换
            * 
            * GetEncoding 方法依赖于基础平台支持大部分代码页。但是，对于下列情况提供系统支持：默认编码，即在执行此方法的计算机的区域设置中指定的编码；Little-Endian Unicode (UTF-16LE)；Big-Endian Unicode (UTF-16BE)；Windows 操作系统 (windows-1252)；UTF-7；UTF-8；ASCII 以及 GB18030（简体中文）。
            *
            *指定下表中列出的其中一个名称以获取具有对应代码页的系统支持的编码。
            *
            * 代码页 名称 
            * 1200 “UTF-16LE”、“utf-16”、“ucs-2”、“unicode”或“ISO-10646-UCS-2” 
            * 1201 “UTF-16BE”或“unicodeFFFE” 
            * 1252 “windows-1252” 
            * 65000 “utf-7”、“csUnicode11UTF7”、“unicode-1-1-utf-7”、“unicode-2-0-utf-7”、“x-unicode-1-1-utf-7”或“x-unicode-2-0-utf-7” 
            * 65001 “utf-8”、“unicode-1-1-utf-8”、“unicode-2-0-utf-8”、“x-unicode-1-1-utf-8”或“x-unicode-2-0-utf-8” 
            * 20127 “us-ascii”、“us”、“ascii”、“ANSI_X3.4-1968”、“ANSI_X3.4-1986”、“cp367”、“csASCII”、“IBM367”、“iso-ir-6”、“ISO646-US”或“ISO_646.irv:1991” 
            * 54936 “GB18030” 
            *
            * 某些平台可能不支持特定的代码页。例如，Windows 98 的美国版本可能不支持日语 Shift-jis 代码页（代码页 932）。这种情况下，GetEncoding 方法将在执行下面的 C# 代码时引发 NotSupportedException：
            *
            * Encoding enc = Encoding.GetEncoding("shift-jis"); 
            *
            * **************************************************************/
            //从base64String中得到原始字符
            try
            {
                return ens.GetString(Convert.FromBase64String(base64String));
            }
            catch
            {
                return base64String;
            }
        }

        /// <summary>
        /// 从base64编码的字符串中还原字符串，支持中文
        /// </summary>
        /// <param name="base64String">base64加密后的字符串</param>
        /// <returns>还原后的文本字符串</returns>
        public static string DecodingForString(string base64String)
        {
            try
            {
                return DecodingForString(base64String, Encoding.GetEncoding(54936));
            }
            catch
            {
                return base64String;
            }
        }
        #endregion

        #region 任意类型的文件进行base64加码
        /// <summary>
        /// 对任意类型的文件进行base64加码
        /// </summary>
        /// <param name="fileName">文件的路径和文件名</param>
        /// <returns>对文件进行base64编码后的字符串</returns>
        public static string EncodingForFile(string fileName)
        {
            var fs = File.OpenRead(fileName);
            var br = new BinaryReader(fs);

            /*System.Byte[] b=new System.Byte[fs.Length];
            fs.Read(b,0,Convert.ToInt32(fs.Length));*/


            var base64String = Convert.ToBase64String(br.ReadBytes((int)fs.Length));


            br.Close();
            fs.Close();
            return base64String;
        }

        /// <summary>
        /// 把经过base64编码的字符串保存为文件
        /// </summary>
        /// <param name="base64String">经base64加码后的字符串</param>
        /// <param name="fileName">保存文件的路径和文件名</param>
        /// <returns>保存文件是否成功</returns>
        public static bool SaveDecodingToFile(string base64String, string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Create);
            var bw = new BinaryWriter(fs);
            bw.Write(Convert.FromBase64String(base64String));
            bw.Close();
            fs.Close();
            return true;
        }
        #endregion

        #region 网络地址一取得文件并转化为base64编码
        /// <summary>
        /// 从网络地址一取得文件并转化为base64编码
        /// </summary>
        /// <param name="url">文件的url地址,一个绝对的url地址</param>
        /// <param name="objWebClient">System.Net.WebClient 对象</param>
        /// <returns></returns>
        private static string EncodingFileFromUrl(string url, WebClient objWebClient)
        {
            return Convert.ToBase64String(objWebClient.DownloadData(url));
        }

        /// <summary>
        /// 从网络地址一取得文件并转化为base64编码
        /// </summary>
        /// <param name="url">文件的url地址,一个绝对的url地址</param>
        /// <returns>将文件转化后的base64字符串</returns>
        public static string EncodingFileFromUrl(string url)
        {
            //System.Net.WebClient myWebClient = new System.Net.WebClient();
            return EncodingFileFromUrl(url, new WebClient());
        }
        #endregion
        
    }
    #endregion

}