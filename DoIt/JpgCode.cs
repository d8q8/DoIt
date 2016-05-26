using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace DoIt
{

    #region 生成JPG验证码
    /// <summary>
    /// 生成验证码
    /// </summary>
    public class JpgCode
    {
        #region 调用方法

        /*=======================================================================================
         * 使用方法：
         *      new JpgCode().CreateImg(Length, Noise, Width, Height, Font, ValidCode)
        =======================================================================================*/
        #endregion

        #region 验证码

        /// <summary>
        /// 产生验证码
        /// </summary>
        /// <param name="length"></param>
        /// <param name="noise"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="font"></param>
        /// <param name="validCode"></param>
        public void CreateImg(int length, int noise, int width, int height, string font, string validCode)
        {
            HttpContext.Current.Response.Expires = -9999;
            HttpContext.Current.Response.AddHeader("pragma", "no-cache");
            HttpContext.Current.Response.AddHeader("cache-ctrol", "no-cache");
            HttpContext.Current.Response.ContentType = "image/jpeg";
            HttpContext.Current.Session["CheckCode"] = validCode.ToLower().Equals("cn") ? GetChinese(length) : GenerateRandomCode(validCode, length);
            // 产生一个图片，并产生SESSION对象
            BmpCode ci = new BmpCode(HttpContext.Current.Session["CheckCode"].ToString(), width, height, font, noise);

            // 清除缓存，输出格式
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "image/jpeg";
            // 生成JPG图片
            ci.Image.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            // 释放资源
            ci.Dispose();
        }
        /// <summary>
        /// 通过调用 Random 类的 Next() 方法
        /// 先获得一个大于或等于 0 而小于 pwdchars 长度的整数
        /// 以该数作为索引值，从可用字符串中随机取字符
        /// 以指定的密码长度为循环次数，依次连接取得的字符
        /// 最后即得到所需的随机密码串了。
        /// </summary>
        /// <param name="pwdchars"></param>
        /// <param name="pwdlen"></param>
        /// <returns></returns>
        private string GenerateRandomCode(string pwdchars, int pwdlen)
        {
            StringBuilder tmpstr = new StringBuilder();
            int iRandNum;
            Random rnd = new Random();
            for (int i = 0; i < pwdlen; i++)
            {
                iRandNum = rnd.Next(pwdchars.Length);
                tmpstr.Append(pwdchars[iRandNum]);
            }
            return tmpstr.ToString();
        }

        private string GenerateRandomCode()
        {
            Random random = new Random();
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        #endregion

        #region  中文验证码

        private string GetChinese(int length)
        {
            Encoding gb = Encoding.GetEncoding("gb2312");
            if (length == 0)
                length = 4;
            object[] bytes = CreateRegionCode(length);
            //string[] resultStr;
            //调用函数产生4个随机中文汉字编码 
            string resultStr = "";
            for (int i = 0; i < length; i++)
            {
                resultStr += gb.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[])));
            }
            return resultStr;

            //根据汉字编码的字节数组解码出中文汉字 
            /*
            object[] bytes=CreateRegionCode(4); 
            string str1=gb.GetString((byte[])Convert.ChangeType(bytes[0], typeof(byte[]))); 
            string str2=gb.GetString((byte[])Convert.ChangeType(bytes[1], typeof(byte[]))); 
            string str3=gb.GetString((byte[])Convert.ChangeType(bytes[2], typeof(byte[]))); 
            string str4=gb.GetString((byte[])Convert.ChangeType(bytes[3], typeof(byte[]))); 
            return str1 + str2 +str3 +str4;
            */
        }


        /**/
        /* 
    此函数在汉字编码范围内随机创建含两个元素的十六进制字节数组，每个字节数组代表一个汉字，并将 
    四个字节数组存储在object数组中。 
    参数：strlength，代表需要产生的汉字个数 
    */
        public static object[] CreateRegionCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素 
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            Random rnd = new Random();

            //定义一个object数组用来 
            object[] bytes = new object[strlength];

            /*/
            /*    每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中 
            /*    每个汉字有四个区位码组成 
            /*    区位码第1位和区位码第2位作为字节数组第一个元素 
            /*    区位码第3位和区位码第4位作为字节数组第二个元素 
            /*/
            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位 
                int r1 = rnd.Next(11, 14);
                string strR1 = rBase[r1].Trim();

                //区位码第2位 
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的 

                //种子避免产生重复值 
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string strR2 = rBase[r2].Trim();

                //区位码第3位 
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string strR3 = rBase[r3].Trim();

                //区位码第4位 
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string strR4 = rBase[r4].Trim();

                //定义两个字节变量存储产生的随机汉字区位码 
                byte byte1 = Convert.ToByte(strR1 + strR2, 16);
                byte byte2 = Convert.ToByte(strR3 + strR4, 16);
                //将两个字节变量存储在字节数组中 
                byte[] strR = new byte[] { byte1, byte2 };

                //将产生的一个汉字的字节数组放入object数组中 
                bytes.SetValue(strR, i);

            }

            return bytes;

        }
        #endregion
    }
    #endregion

    #region 验证码产生类
    /// <summary>
    /// 产生验证码
    /// </summary>
    public class BmpCode
    {
        // 初始化属性
        private string _text;
        private int _width;
        private int _height;
        private string _familyName;
        private Bitmap _image;
        private int _noise = 30;
        // 产生随机数
        private Random _random = new Random();
        // 验证码
        public string Text
        {
            get { return this._text; }
        }
        public Bitmap Image
        {
            get { return this._image; }
        }
        public int Width
        {
            get { return this._width; }
        }
        public int Height
        {
            get { return this._height; }
        }

        /// <summary>
        /// 产生验证码
        /// </summary>
        /// <param name="s">字符</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public BmpCode(string s, int width, int height)
        {
            this._text = s;
            this.SetDimensions(width, height);
            this.GenerateImage();
        }

        /// <summary>
        /// 产生验证码，带字体
        /// </summary>
        /// <param name="s">字符</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="familyName">字体</param>
        public BmpCode(string s, int width, int height, string familyName)
        {
            this._text = s;
            this.SetDimensions(width, height);
            this.SetFamilyName(familyName);
            this.GenerateImage();
        }
        /// <summary>
        /// 产生验证码，带杂点
        /// </summary>
        /// <param name="s"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="familyName"></param>
        /// <param name="noise"></param>
        public BmpCode(string s, int width, int height, string familyName, int noise)
        {
            this._text = s;
            this._noise = noise;
            this.SetDimensions(width, height);
            this.SetFamilyName(familyName);
            this.GenerateImage();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~BmpCode()
        {
            Dispose(false);
        }

        /// <summary>
        /// 系统回收
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        /// <summary>
        /// 自定义回收资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                // Dispose of the bitmap.
                this._image.Dispose();
        }

        /// <summary>
        /// 设置图片宽和高
        /// </summary>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        private void SetDimensions(int width, int height)
        {
            // Check the width and height.
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero.");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero.");
            this._width = width;
            this._height = height;
        }

        /// <summary>
        /// 设置字体函数
        /// </summary>
        /// <param name="familyName">字体</param>
        private void SetFamilyName(string familyName)
        {
            // 如果字体没有安装，默认为系统字体
            try
            {
                //Font font = new Font(this.familyName, 12F);
                this._familyName = familyName;
                Font font = new Font(this._familyName, 12F);
                font.Dispose();
            }
            catch
            {
                this._familyName = System.Drawing.FontFamily.GenericSerif.Name;
            }
        }

        /// <summary>
        /// 产生BMP图片
        /// </summary>
        private void GenerateImage()
        {
            // 生成一个32位位图
            Bitmap bitmap = new Bitmap(this._width, this._height, PixelFormat.Format32bppArgb);

            // 创建一个GDI+绘画对象
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this._width, this._height);

            // 填充背景，产生笔刷														
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.LargeGrid, Color.LightGray, Color.White);
            g.FillRectangle(hatchBrush, rect);

            // 设置文本字体
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // 调整字体大小直到充满图片
            do
            {
                fontSize--;
                font = new Font(this._familyName, fontSize, FontStyle.Bold);
                size = g.MeasureString(this._text, font);
            } while (size.Width > rect.Width);

            // 字符串格式化
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // 产生连接线
            GraphicsPath path = new GraphicsPath();
            path.AddString(this._text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            float v = 4F;
            PointF[] points =
			{
				new PointF(this._random.Next(rect.Width) / v, this._random.Next(rect.Height) / v),
				new PointF(rect.Width - this._random.Next(rect.Width) / v, this._random.Next(rect.Height) / v),
				new PointF(this._random.Next(rect.Width) / v, rect.Height - this._random.Next(rect.Height) / v),
				new PointF(rect.Width - this._random.Next(rect.Width) / v, rect.Height - this._random.Next(rect.Height) / v)
			};
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // 画图										Color.LightGray ,DarkGray
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.Black, Color.Black);
            g.FillPath(hatchBrush, path);

            // 增加一些随机杂点
            int m = Math.Max(rect.Width, rect.Height);
            //for (int i = 0; i < (int) (rect.Width * rect.Height / 30F); i++)  //杂点 30F 数值越大，杂点越少

            for (int i = 0; i < (int)(rect.Width * rect.Height / _noise); i++)
            {
                int x = this._random.Next(rect.Width);
                int y = this._random.Next(rect.Height);
                int w = this._random.Next(m / 50);
                int h = this._random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // 清除资源
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            // 产生图片
            this._image = bitmap;
        }
    }

    #endregion

}
