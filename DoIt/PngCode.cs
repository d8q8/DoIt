using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Drawing;

namespace DoIt
{
    public class PngCode
    {

        #region 调用方法
        /*******************************************************************************
         * 调用实例：（ashx文件）
         * //特别注意，如不加，单击验证图片＇看不清换一张＇，无效果。
         * 
         *      context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
         *      PngCode.CreateCheckCodeImage(PngCode.GenerateCheckCode(context, 4), context);
         * 
         * ****************************************************************************/
        #endregion

        #region 生成验证码

        /// <summary>
        /// 产生验证码保存COOKIES值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GenerateCheckCode(HttpContext context, int num)
        {
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < num; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else if (number % 3 == 0)
                    code = (char)('A' + (char)(number % 26));
                else
                    code = (char)('a' + (char)(number % 26));
                checkCode += code.ToString();
            }
            context.Response.Cookies["CheckCode"].Value = checkCode;
            //context.Response.Cookies.Add(new HttpCookie("CheckCode", checkCode));//也可以存到Session里．
            //context.Session["CheckCode"] = checkCode;

            return checkCode;
        }
        /// <summary>
        /// 产生验证码图片
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="context"></param>
        public static void CreateCheckCodeImage(string checkCode, HttpContext context)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 13.5)), 24);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.FromArgb(random.Next()));

                //画图片的背景噪音线
                for (int i = 0; i < 5; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.FromArgb(random.Next())), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Verdana", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Regular));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.FromArgb(189, 80, 179), Color.FromArgb(15, 206, 179), 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                for (int i = 0; i < 10; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                Pen pen = new Pen(Color.FromArgb(204, 204, 204), 1);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                g.DrawRectangle(pen, 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                context.Response.ClearContent();
                context.Response.ContentType = "image/Png";
                context.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        #endregion
    }
}
