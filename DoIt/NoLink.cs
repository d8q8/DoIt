using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Web;
using System.Text.RegularExpressions;
//using System.Configuration;

namespace DoIt
{
    public class NoLink
    {
        public NoLink()
        {
            // 
            // TODO: 在此处添加构造函数逻辑 
            // 
        }

        #region 调用实例

        /*************************************************************************
        * 
        * 防盗链IHttpHandler 
        * 
        * 
        * 增加了对文件关键字的选择(即仅对文件名存在某些关键字或不存在某些关键字进行过滤) 
        * 设置web.config中<appSettings>节以下值 
        * string eWebapp_NoLink 如果文件名符合该正确表态式将进行过滤(不设置对所有进行过滤) 
        * string eWebapp_AllowLink 如果文件名符合该正确表态式将不进行过滤(优先权高于AllowLink,不设置则服从AllowLink) 
        * bool eWebapp_ AllowOnlyFile 如果为False,(默认true)则不允许用户直接对该文件进行访问建议为true 
        * 
        * 
        * :)以下设置均可省略,设置只是为了增加灵活性与体验 
        * eWebapp_NoLink_Message 错误信息提示:默认为 链接来源于：域名 
        * eWebapp_Error_Width 错误信息提示图片宽 
        * eWebapp_Error_Height 错误信息提示图片高 
        * 
        * 调用方法
        * 
        public void ProcessRequest(HttpContext context)
        {
            string eWebapp_NoLink = ConfigurationSettings.AppSettings["eWebapp_NoLink"];
            string eWebapp_AllowLink = ConfigurationSettings.AppSettings["eWebapp_AllowLink"];
            bool eWebapp_AllowOnlyFile = true;
            bool error = false;
            try
            {
                eWebapp_AllowOnlyFile = Convert.ToBoolean(ConfigurationSettings.AppSettings["eWebapp_AllowOnlyFile"]);
            }
            catch
            {
                eWebapp_AllowOnlyFile = true;
            }

            string eWebapp_NoLink_Message = ConfigurationSettings.AppSettings["eWebapp_NoLink_Message"];
            string eWebapp_Error_Width = ConfigurationSettings.AppSettings["eWebapp_Error_Width"];
            string eWebapp_Error_Height = ConfigurationSettings.AppSettings["eWebapp_Error_Height"];

            string myDomain = string.Empty;

            error = errorLink(context, eWebapp_NoLink, eWebapp_AllowLink, eWebapp_AllowOnlyFile, out myDomain);

            if (Empty(eWebapp_NoLink_Message))
            {
                eWebapp_NoLink_Message = "链接来源于：" + myDomain;
            }

            if (error)
            {
                Jpg(context.Response, eWebapp_NoLink_Message, eWebapp_Error_Width, eWebapp_Error_Height);
            }
            else
            {
                Real(context.Response, context.Request);
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
            
        *********************************************************************************/
        #endregion

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="msg"></param>
        /// <param name="wid"></param>
        /// <param name="hei"></param>
        public void Jpg(HttpResponse response, string msg, string wid, string hei)
        {

            int myErrorWidth = msg.Length * 12;
            int myErrorHeight = 20;
            try
            {
                int _myErrorWidth = Convert.ToInt32(wid);
                if (_myErrorWidth > 0)
                {
                    myErrorWidth = _myErrorWidth;
                }
            }
            catch{}
            try
            {
                int _myErrorHeight = Convert.ToInt32(hei);
                if (_myErrorHeight > 0)
                {
                    myErrorHeight = _myErrorHeight;
                }
            }
            catch{}
            Bitmap img = null;
            Graphics g = null;
            MemoryStream ms = null;
            img = new Bitmap(myErrorWidth, myErrorHeight);
            g = Graphics.FromImage(img);
            //文字高质量
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //图片高质量
            //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; 
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; 
            
            //清空图片背景色
            g.Clear(Color.White);
            Font f = new Font("Verdana", 9);
            SolidBrush s = new SolidBrush(Color.Red);
            g.DrawString(msg, f, s, 3, 3);

            //画图片的边框线
            Pen pen = new Pen(Color.FromArgb(51, 51, 51), 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            g.DrawRectangle(pen, 0, 0, img.Width - 1, img.Height - 1);

            ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            response.ClearContent();
            response.ContentType = "image/Png";
            response.BinaryWrite(ms.ToArray());
            g.Dispose();
            img.Dispose();
            response.End();
        }

        /// <summary> 
        /// 输出真实文件 
        /// </summary> 
        /// <param name="response"></param> 
        /// <param name="context"></param> 
        public void Real(HttpResponse response, HttpRequest request)
        {
            FileInfo file = new System.IO.FileInfo(request.PhysicalPath);
            response.Clear();
            response.AddHeader("Content-Disposition", "filename=" + file.Name);
            response.AddHeader("Content-Length", file.Length.ToString());
            string fileExtension = file.Extension.ToLower();

            //这里选择输出的文件格式 
            switch (fileExtension)
            {
                case "mp3":
                    response.ContentType = "audio/mpeg3";
                    break;
                case "mpeg":
                    response.ContentType = "video/mpeg";
                    break;
                case "jpg":
                    response.ContentType = "image/jpeg";
                    break;
                case "bmp":
                    response.ContentType = "image/bmp";
                    break;
                case "gif":
                    response.ContentType = "image/gif";
                    break;
                case "doc":
                    response.ContentType = "application/msword";
                    break;
                case "css":
                    response.ContentType = "text/css";
                    break;
                default:
                    response.ContentType = "application/octet-stream";
                    break;
            }
            response.WriteFile(file.FullName);
            response.End();
        }

        /// <summary> 
        /// 确认字符串是否为空 
        /// </summary> 
        /// <param name="value"></param> 
        /// <returns></returns> 
        private bool Empty(string value)
        {
            return string.IsNullOrEmpty(value) || value.Equals("");
        }

        /// <summary> 
        /// 检查是否是非法链接 
        /// </summary> 
        /// <param name="context"></param> 
        /// <param name="_myDomain"></param> 
        /// <returns></returns> 
        public bool ErrorLink(HttpContext context, string eWebappNoLink, string eWebappAllowLink, bool eWebappAllowOnlyFile, out string _myDomain)
        {
            HttpResponse response = context.Response;
            string myDomain = context.Request.ServerVariables["SERVER_NAME"];
            _myDomain = myDomain;
            string myDomainIp = context.Request.UserHostAddress;

            if (context.Request.UrlReferrer != null)
            {

                //判定referDomain是否存在网站的IP或域名 
                string referDomain = context.Request.UrlReferrer.AbsoluteUri.Replace(context.Request.UrlReferrer.AbsolutePath, "");
                string myPath = context.Request.RawUrl;

                if (referDomain.IndexOf(myDomainIp) >= 0 | referDomain.IndexOf(myDomain) >= 0)
                {
                    return false;
                }
                else
                {
                    //这里使用正则表达对规则进行匹配 
                    try
                    {
                        Regex myRegex;
                        //检查允许匹配 
                        if (!Empty(eWebappAllowLink))
                        {
                            myRegex = new Regex(eWebappAllowLink);
                            if (myRegex.IsMatch(myPath))
                            {
                                return false;
                            }
                        }

                        //检查禁止匹配 
                        if (!Empty(eWebappNoLink))
                        {
                            myRegex = new Regex(eWebappNoLink);
                            if (myRegex.IsMatch(myPath))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                    catch
                    {
                        //如果匹配出错，链接错误 
                        return true;
                    }
                }
            }
            else
            {
                //是否允许直接访问文件 
                if (eWebappAllowOnlyFile)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
