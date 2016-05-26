using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace DoIt
{
    public class GatherUtil
    {
        public GatherUtil()
        {
            // 
            // TODO: 在此处添加构造函数逻辑 
            // 
        }
        /// <summary>
        /// 要采集的网页的连接地址
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="chargest">编码</param>
        /// <param name="path">保存路径</param>
        /// <returns></returns>
        public static string CaijiByUrl(string url, string chargest, string path)
        {
            string str = GetSourceTextByUrl(url, chargest);
            ArrayList lib = new ArrayList();
            //int i = 0;
            //根据url取得网站域名 
            Uri uri = new Uri(url);
            //Scheme或者协议，一般为http,Host为取得域名 
            string baseurl = uri.Scheme + "://" + uri.Host + "/";
            //提取出url，包括src等信息 
            //\S匹配任何非空白字符 
            Regex g = new Regex(@"(src=(""|\')\S+\.(gif|jpg|png|bmp)(""|\'))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            MatchCollection m = g.Matches(str);
            foreach (Match math in m)
            {
                //已经提取到图片的路径了，但还需要分绝对路径，相对路径，以及后缀名是否为图片，因为可能为.asp,.aspx这些，比如验证码图片 
                string imgUrl = math.Groups[0].Value.ToLower();//转成小写,=号之间可能有不定的空格 
                //去除src与单引号，双引号 
                imgUrl = imgUrl.Replace("src", "");
                imgUrl = imgUrl.Replace("\"", "");
                imgUrl = imgUrl.Replace("'", "");
                imgUrl = imgUrl.Replace("=", "");
                imgUrl = imgUrl.Trim();
                //路径处理 
                if (imgUrl.Substring(0, 4) != "http")
                {
                    //需要判断是否是绝对路径还是相对路径 
                    if (imgUrl.Substring(0, 1) == "/")
                    {
                        imgUrl = baseurl + imgUrl;
                    }
                    else
                    {
                        imgUrl = url.Substring(0, url.LastIndexOf("/") + 1) + imgUrl;
                    }
                }
                //判断元素是否已经存在,-1为不存在 
                if (lib.IndexOf(imgUrl) == -1)
                {
                    lib.Add(imgUrl);
                }
            }
            string str_ = string.Empty;
            WebClient client = new WebClient();
            for (int j = 0; j < lib.Count; j++)
            {
                string savepath = path + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Minute + DateTime.Now.Second + j + lib[j].ToString().Substring((lib[j].ToString().Length) - 4, 4);
                try
                {
                    client.DownloadFile(new Uri(lib[j].ToString()), savepath);
                    str_ += lib[j].ToString() + "<br /> 保存路径为：" + savepath + "<br /><br />";
                }
                catch (Exception e)
                {
                    str_ += e.Message;
                }
            }
            return str_;
        }
        /// <summary>
        /// 获取页面代码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="chargest"></param>
        /// <returns></returns>
        public static string GetSourceTextByUrl(string url, string chargest)
        {
            WebRequest request = WebRequest.Create(url);
            request.Timeout = 20000;//20秒超时 
            WebResponse response = request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream, Encoding.GetEncoding(chargest));
            return sr.ReadToEnd();
        }
        /// <summary>
        /// 判断远程图片是否存在
        /// </summary>
        /// <param name="curl"></param>
        /// <returns></returns>
        private static int GetUrlError(string curl)
        {
            int num = 200;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(curl));
            ServicePointManager.Expect100Continue = false;
            try
            {
                ((HttpWebResponse)request.GetResponse()).Close();
            }
            catch (WebException exception)
            {
                if (exception.Status != WebExceptionStatus.ProtocolError)
                {
                    return num;
                }
                if (exception.Message.IndexOf("500 ") > 0)
                {
                    return 500;
                }
                if (exception.Message.IndexOf("401 ") > 0)
                {
                    return 401;
                }
                if (exception.Message.IndexOf("404") > 0)
                {
                    num = 404;
                }
            }
            return num;
        }

    }
}
