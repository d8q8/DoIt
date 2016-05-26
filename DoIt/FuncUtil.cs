using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using static System.Web.HttpContext;

namespace DoIt
{
    public class FuncUtil
    {

        #region IP处理
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            var result = Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = Current.Request.UserHostAddress;
            }
            return result;
        }
        /// <summary>
        /// 截断IP
        /// </summary>
        /// <param name="ip">客户端IP</param>
        /// <returns></returns>
        public static string CutIp(string ip)
        {
            return ip.Substring(0, ip.LastIndexOf('.')) + ".*";
        }
        /// <summary>
        /// 获取IP省和地址名称
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="ip"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string Ipname(string filepath, string ip, int flag)
        {
            var dbip = Current.Server.MapPath(filepath);
            var qq = new QqWry(dbip);
            var loca = qq.SearchIpLocation(ip);
            return flag == 0 ? loca.Country : (flag == 1 ? loca.Area : (loca.Country + loca.Area));
        }
        #endregion

        #region 加密处理
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="code">加密长度</param>
        /// <returns></returns>
        public static string Md5(string str, int code)
        {
            //var hashPasswordForStoringInConfigFile = HashPasswordForStoringInConfigFile(str, "MD5");
            var md5 = MD5.Create();
            var hashPasswordForStoringInConfigFile = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", null);
            if (hashPasswordForStoringInConfigFile != null)
                return code == 16 ? hashPasswordForStoringInConfigFile.ToLower().Substring(8, 16) : hashPasswordForStoringInConfigFile.ToLower();
            return null;
        }

        /// <summary>
        /// 异或加密解密
        /// </summary>
        /// <param name="p"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptDecryptStr(string p, byte key)
        {
            var bytes = Encoding.Default.GetBytes(p);
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ key);
            }
            return Encoding.Default.GetString(bytes);
        }
        #endregion

        #region HTML处理
        ///   <summary>
        ///   移除HTML标签
        ///   </summary>
        ///   <param name="str">字符串</param>
        ///   <returns></returns>
        private static string ParseTags(string str)
        {
            return Regex.Replace(str, "<[^>]*>", "");
        }

        /// <summary>
        /// 过滤HTML代码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string Dohtml(string html)
        {
            var regex1 = new Regex(@"<script[\s\S]+</script *>", RegexOptions.IgnoreCase);
            var regex2 = new Regex(@" href *= *[\s\S]*script *:", RegexOptions.IgnoreCase);
            var regex3 = new Regex(@" no[\s\S]*=", RegexOptions.IgnoreCase);
            var regex4 = new Regex(@"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase);
            var regex5 = new Regex(@"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase);
            var regex6 = new Regex(@"\<img[^\>]+\>", RegexOptions.IgnoreCase);
            var regex7 = new Regex(@"</p>", RegexOptions.IgnoreCase);
            var regex8 = new Regex(@"<p>", RegexOptions.IgnoreCase);
            var regex9 = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记 
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性 
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
            html = regex4.Replace(html, ""); //过滤iframe 
            html = regex5.Replace(html, ""); //过滤frameset 
            html = regex6.Replace(html, ""); //过滤frameset 
            html = regex7.Replace(html, ""); //过滤frameset 
            html = regex8.Replace(html, ""); //过滤frameset 
            html = regex9.Replace(html, "");
            html = html.Replace("&nbsp;", " ");
            html = html.Replace(" ", " ");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            html = html.Replace("\r\n\r\n", "");
            html = html.Replace("\r\n", "<br />");
            return html;
        }
        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="strHtml">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string StripHtml(string strHtml)
        {
            string[] aryReg ={
          @"<script[^>]*?>.*?</script>", 
          @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
          @"([\r\n])[\s]+",
          @"&(quot|#34);",
          @"&(amp|#38);",
          @"&(lt|#60);",
          @"&(gt|#62);", 
          @"&(nbsp|#160);", 
          @"&(iexcl|#161);",
          @"&(cent|#162);",
          @"&(pound|#163);",
          @"&(copy|#169);",
          @"&#(\d+);",
          @"-->",
          @"<!--.*\n"
        
        };
            string[] aryRep = {
          "",
          "",
          "",
          "\"",
          "&",
          "<",
          ">",
          " ",
          "\xa1",//chr(161),
          "\xa2",//chr(162),
          "\xa3",//chr(163),
          "\xa9",//chr(169),
          "",
          "\r\n",
          ""
          };
            var strOutput = strHtml;
            for (var i = 0; i < aryReg.Length; i++)
            {
                var regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");

            return strOutput;
        }
        #endregion

        #region 字符串转换
        /// <summary>
        /// 过滤非法字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public string filterStr(string str, string temp)
        {
            var t = temp.Split(',');
            return t.Aggregate(str, (current, t1) => current.Replace(t1, new string('*', t1.Length)));
        }

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        private static bool IsNumeric(string str)
        {
            var ch = new char[str.Length];
            ch = str.ToCharArray();
            return ch.All(t => t >= 48 && t <= 57);
        }
        /// <summary>
        /// 截断字符插入隔断字符
        /// </summary>
        /// <returns></returns>
        public string CutString(string str, int len, string endstr)
        {
            string tempstr;
            var pstr = string.Empty;
            if (str.Length > len)
            {
                var tt = int.Parse((str.Length / len).ToString());
                for (var i = 0; i < tt; i++)
                {
                    pstr += str.Substring(i * len, len) + endstr;
                }
                tempstr = pstr;
            }
            else
            {
                tempstr = str;
            }
            return tempstr;
        }
        /// <summary>
        /// 截断标题字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string cutStr(string str, int len)
        {
            var tempstr = ParseTags(str);
            var length = tempstr.Length;
            var num2 = 0;
            var num3 = length;
            var num4 = 0;
            var chArray = tempstr.ToCharArray();
            for (var i = 0; i < chArray.Length; i++)
            {
                if (Convert.ToInt32(chArray[i]) > 255)
                {
                    num2 += 2;
                }
                else
                {
                    num2++;
                    num4++;
                }
                if (num2 < len) continue;
                if ((num3 % 2) == 0)
                {
                    num3 = (num4%2) == 0 ? i + 1 : i;
                }
                else
                {
                    num3 = i + 1;
                }
                break;
            }
            var str2 = tempstr.Substring(0, num3);
            if (tempstr.Length > num3)
            {
                return (str2 + "…");
            }
            return str2;
        }
        /// <summary>
        /// 截断字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">长度</param>
        /// <param name="endstr">尾部字符</param>
        /// <returns></returns>
        public static string cutStr(string str, int length, string endstr)
        {
            //判断字串是否为空
            if (str == null)
            {
                return "";
            }
            str.Replace("\r\n", "");
            var reg = new Regex("(<([^>]*)>)");
            var m = reg.Match(str);
            str = Regex.Replace(str, m.Groups[0].Value, "", RegexOptions.IgnoreCase);
            if (Encoding.Default.GetByteCount(str) <= length) return str;
            //初始化
            int i = 0, j = 0;
            //为汉字或全脚符号长度加2否则加1
            foreach (var Char in str)
            {
                if ((int)Char > 127)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }
                if (i > length)
                {
                    str = str.Substring(0, j - 1) + endstr;
                    break;
                }
                j++;
            }
            return str;
        }
        /// <summary>
        /// 转换html字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Htmlcode(string str)
        {
            if (str == "")
            {
                return str;
            }
            else
            {
                str = str.Replace(" ", " ");
                str = str.Replace("\n\r", "<br />");
            }
            return str;
        }

        /// <summary>
        /// 反转html字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Htmlencode(string str)
        {
            if (str == "")
            {
                return str;
            }
            else
            {
                str = str.Replace("", " ");
                str = str.Replace("<br />", "\n\r");
            }
            return str;
        }

        /// <summary>
        /// 安全字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string GetStr(string str)
        {
            var tempstr = str.Trim().Replace("'", "");
            //tempstr = tempstr.Replace(";", "");
            //tempstr = tempstr.Replace("-", "");
            return tempstr;
        }

        /// <summary>
        /// 特殊字符过滤函数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string filterStr(string str)
        {
            //如果字符串为空，直接返回。
            if (str == "") return str;
            str = str.Replace("'", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("%", "");
            str = str.Replace("'delete", "");
            str = str.Replace("''", "");
            str = str.Replace("\"\"", "");
            str = str.Replace(",", "");
            str = str.Replace(".", "");
            str = str.Replace(">=", "");
            str = str.Replace("=<", "");
            str = str.Replace("-", "");
            str = str.Replace("_", "");
            str = str.Replace(";", "");
            str = str.Replace("||", "");
            str = str.Replace("[", "");
            str = str.Replace("]", "");
            str = str.Replace("&", "");
            str = str.Replace("/", "");
            str = str.Replace("-", "");
            str = str.Replace("|", "");
            str = str.Replace("?", "");
            str = str.Replace(">?", "");
            str = str.Replace("?<", "");
            str = str.Replace(" ", "");
            return str;
        }
        #endregion

        #region 获取随机处理
        /// <summary>
        /// 获取时间+随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static string GetRandom(int min, int max)
        {
            var ran = new Random();
            var tempstr = GetDate(DateTime.Now, 14) + ran.Next(min, max);
            return tempstr;
        }
        #endregion

        #region 重写传参处理
        /// <summary>
        /// 重写GET传参
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Q(string str)
        {
            return Current.Request.QueryString[str] != null ? Current.Request.QueryString[str].ToString() : string.Empty;
        }

        /// <summary>
        /// 获取GET集合
        /// </summary>
        /// <returns></returns>
        public static string Q()
        {
            return Current.Request.QueryString.ToString();
        }

        /// <summary>
        /// 重写POST传参
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string F(string str)
        {
            return Current.Request.Form[str] != null ? Current.Request.Form[str].ToString() : string.Empty;
        }

        /// <summary>
        /// 重写GET与POST传参
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string P(string str)
        {
            return Current.Request.Params[str] != null ? Current.Request.Params[str].ToString() : string.Empty;
        }

        /// <summary>
        /// 重写COOKIES传参
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string C(string str)
        {
            return HC(str) != null ? HC(str).Value : string.Empty;
        }

        /// <summary>
        /// 重写COOKIES并获取指定值
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string C(HttpCookie cookies, string str)
        {
            return cookies != null ? cookies.Values[str] : string.Empty;
        }

        /// <summary>
        /// 重写获取COOKIES传参
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static void C(string str, string val)
        {
            var httpCookie = Current.Response.Cookies[str];
            if (httpCookie != null)
                httpCookie.Value = val;
        }

        /// <summary>
        /// 获取COOKIES多值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string HC(string str, string val)
        {
            return Current.Request.Cookies[str] != null ? Current.Request.Cookies[str].Values[val].ToString() : null;
        }

        /// <summary>
        /// 重写HTTPCOOKIE传参
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static HttpCookie HC(string str)
        {
            return Current.Request.Cookies[str] ?? null;
        }

        /// <summary>
        /// 重写COOKIES获取单值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static HttpCookie Wc(string str)
        {
            return Current.Response.Cookies[str] ?? null;
        }

        /// <summary>
        /// 重写SESSION传参
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string S(string str)
        {
            return Cs(str) != null ? Cs(str).ToString() : string.Empty;
        }

        /// <summary>
        /// 重写SESSION对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static object Cs(string str)
        {
            return Current.Session[str] ?? null;
        }

        /// <summary>
        /// 重写跳转传参
        /// </summary>
        /// <param name="str"></param>
        public static void R(string str)
        {
            Current.Response.Redirect(str);
        }
        /// <summary>
        /// 重写地址编码
        /// </summary>
        /// <param name="str"></param>
        private static string Ue(string str)
        {
            return Current.Server.UrlEncode(str);
        }
        /// <summary>
        /// 重写地址解码
        /// </summary>
        /// <param name="str"></param>
        public static string Ud(string str)
        {
            return Current.Server.UrlDecode(str);
        }
        /// <summary>
        /// 重写输出
        /// </summary>
        /// <param name="str"></param>
        public static void W(object str)
        {
            Current.Response.Write(str);
        }
        /// <summary>
        /// 获取地址路径
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string U()
        {
            return Current.Request.UrlReferrer?.ToString() ?? Current.Request.Url.PathAndQuery.ToString();
        }

        #endregion

        #region 错误处理

        /// <summary>
        /// 前台跳转页面
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static void Info(string str)
        {
            R("error.aspx?info=" + Ue(str));
        }

        /// <summary>
        /// 后台跳转页面
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static void Goto(string str)
        {
            R("../admin/error.aspx?info=" + Ue(str));
        }

        /// <summary>
        /// 判断是否登录成功
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool AdminLogin(HttpCookie c)
        {
            return c != null;
        }
        public static bool AdminLogin(string c)
        {
            return c != null;
        }

        /// <summary>
        /// 判断是否登录成功并跳转
        /// </summary>
        /// <param name="c"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static void AdminLogin(HttpCookie c, string url)
        {
            if (c == null)
            {
                R(url);
            }
        }
        public static void AdminLogin(string c, string url)
        {
            if (c == null)
            {
                R(url);
            }
        }

        /// <summary>
        /// 登录跳转页面
        /// </summary>
        /// <param name="c"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static void MyUrl(HttpCookie c, string map)
        {
            if (c == null)
            {
                R(MyAddress((HttpCookie) null, map));
            }
        }
        public static void MyUrl(string c, string map)
        {
            if (c == null)
            {
                R(MyAddress((string) null, map));
            }
        }

        /// <summary>
        /// 登录跳转页面地址
        /// </summary>
        /// <param name="c"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        private static string MyAddress(HttpCookie c, string map)
        {
            return c == null ? ($"Login.aspx?Url={map}") : map;
        }

        private static string MyAddress(string c, string map)
        {
            return c == null ? ($"Login.aspx?Url={map}") : map;
        }

        /// <summary>
        /// 判断用户是否包含该权限
        /// </summary>
        /// <param name="c"></param>
        /// <param name="rights"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static bool AdminPower(HttpCookie c, string rights, string power)
        {
            var a = !c.Values[rights].ToString().EndsWith(",") ? $",{c.Values[rights].ToString()}," : $",{c.Values[rights].ToString()}";
            var b = $",{power},";
            return a.IndexOf(b, StringComparison.Ordinal) > -1;
        }
        public static bool AdminPower(string c, string power)
        {
            var a = !c.EndsWith(",") ? $",{c}," : $",{c}";
            var b = $",{power},";
            return a.IndexOf(b, StringComparison.Ordinal) > -1;
        }
        #endregion

        #region 时间转换
        /// <summary>
        /// 时间转换函数
        /// </summary>
        /// <returns></returns>
        public static string GetDate(DateTime datetime, int flag)
        {
            string tempstr;
            var y = datetime.Year.ToString();
            var m = datetime.Month.ToString();
            var d = datetime.Day.ToString();
            var h = datetime.Hour.ToString();
            var f = datetime.Minute.ToString();
            var s = datetime.Second.ToString();
            var week = datetime.DayOfWeek.ToString();
            var w = string.Empty;
            switch (week)
            {
                case "Monday":
                    w = "星期一";
                    break;
                case "Tuesday":
                    w = "星期二";
                    break;
                case "Wednesday":
                    w = "星期三";
                    break;
                case "Thursday":
                    w = "星期四";
                    break;
                case "Friday":
                    w = "星期五";
                    break;
                case "Saturday":
                    w = "星期六";
                    break;
                case "Sunday":
                    w = "星期日";
                    break;
            }
            if (m.Length == 1) m = "0" + m;
            if (d.Length == 1) d = "0" + d;
            if (h.Length == 1) h = "0" + h;
            if (f.Length == 1) f = "0" + f;
            if (s.Length == 1) s = "0" + s;
            switch (flag)
            {
                case 0: tempstr = $"{y}-{m}-{d} {h}:{f}";
                    break;
                case 1: tempstr = $"{y}-{m}-{d}";
                    break;
                case 2: tempstr = $"{m}-{d}";
                    break;
                case 3: tempstr = $"{h} {f}";
                    break;
                case 4: tempstr = $"{y}-{m}-{d} {h}:{f}:{s}";
                    break;
                case 5: tempstr = $"{y}年{m}月{d}日 {h}:{f}:{s}";
                    break;
                case 6: tempstr = $"{m}月 ({h}:{f})";
                    break;
                case 7: tempstr = $"{y}年{m}月{d}日";
                    break;
                case 8: tempstr = $"{h}:{f}:{s}";
                    break;
                case 9: tempstr = $"{m}月{d}日";
                    break;
                case 10: tempstr = $"{m}月";
                    break;
                case 11: tempstr = $"{y}{m}{d}{h}{f}{s}{new Random().Next(10000, 99999)}";
                    break;
                case 12: tempstr = $"[{m}-{d}]";
                    break;
                case 13: tempstr = $"{y}年{m}月";
                    break;
                case 14: tempstr = $"{y}{m}{d}{h}{f}{s}";
                    break;
                case 15: tempstr = $"{y}年{m}月{d}日 {h}:{f}";
                    break;
                case 16: tempstr = $"{m}月{d}日 {w}";
                    break;
                case 17: tempstr = $"{y}年{m}月{d}日 {w}";
                    break;
                default: tempstr = $"{y}-{m}-{d} {h}:{f}:{s}";
                    break;
            }
            return tempstr;
        }
        #endregion

        #region 路径处理
        /// <summary>
        /// 获取网站根目录
        /// </summary>
        /// <returns></returns>
        public static string GetRootUri()
        {
            var appPath = string.Empty;
            var httpCurrent = Current;
            if (httpCurrent == null) return appPath;
            var req = httpCurrent.Request;

            var urlAuthority = req.Url.GetLeftPart(UriPartial.Authority);
            if (req.ApplicationPath == null || req.ApplicationPath == "/")
                //直接安装在   Web   站点   
                appPath = urlAuthority;
            else
            //安装在虚拟子目录下   
                appPath = urlAuthority + req.ApplicationPath;
            return appPath;
        }
        /// <summary>
        /// 获取父路径
        /// </summary>
        /// <returns></returns>
        public static string GetParentDirectory(string filepath)
        {
            var context = Current;
            var path = context.Session["path"].ToString();
            switch (path)
            {
                case "./":
                    return ("../");
                case "/":
                    return filepath;
                default:
                    if (path.LastIndexOf("/", StringComparison.Ordinal) == path.Length - 1)
                    {
                        path = path.Remove(path.LastIndexOf("/", StringComparison.Ordinal), (path.Length - path.LastIndexOf("/", StringComparison.Ordinal)));
                    }
                    try
                    {
                        return (path + "/");
                    }
                    catch
                    {
                        return filepath;
                    }
            }
        }
        /// <summary>
        /// 获取文件及文件夹大小
        /// </summary>
        /// <param name="dirp"></param>
        /// <returns></returns>
        public static int GetSize(string dirp)
        {
            var total = 0;
            var mydir = new DirectoryInfo(dirp);
            foreach (var fsi in mydir.GetFileSystemInfos())
            {
                var info = fsi as FileInfo;
                if (info != null)
                {
                    var fi = info;
                    total += int.Parse(fi.Length.ToString());
                }
                else
                {
                    var di = (DirectoryInfo)fsi;
                    var newDir = di.FullName;
                    GetSize(newDir);
                }
            }
            return total;
        }

        /// <summary>
        /// 递归获取目录大小
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double DirSize(DirectoryInfo d)
        {
            //   Add   file   sizes.   
            var fis = d.GetFiles();
            var size = fis.Aggregate<FileInfo, double>(0, (current, fi) => current + fi.Length);
            //   Add   subdirectory   sizes.   
            var dis = d.GetDirectories();
            size += dis.Sum(di => DirSize(di));
            return (size);
        }

        /// <summary>
        /// 大小转换+单位
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string upload_Size(double size)
        {
            string str;
            if (size < 1024.0)
            {
                str = Math.Round(double.Parse(size.ToString(CultureInfo.InvariantCulture)), 2) + " B";
            }
            else if (size >= 1024.0 && size < (1024 * 1024.0))
            {
                str = Math.Round(double.Parse((size / 1024.0).ToString(CultureInfo.InvariantCulture)), 2) + " KB";
            }
            else if (size >= (1024 * 1024.0) && size < (1024 * 1024 * 1024.0))
            {
                str = Math.Round(double.Parse((size / (1024 * 1024.0)).ToString(CultureInfo.InvariantCulture)), 2) + " MB";
            }
            else if (size >= (1024 * 1024 * 1024.0) && size < (1024 * 1024 * 1024 * 1024.0))
            {
                str = Math.Round(double.Parse((size / (1024 * 1024 * 1024.0)).ToString(CultureInfo.InvariantCulture)), 2) + " GB";
            }
            else
            {
                str = Math.Round(double.Parse((size / (1024 * 1024 * 1024 * 1024.0)).ToString(CultureInfo.InvariantCulture)), 2) + "TB";
            }
            return str;
        }
        #endregion

        #region 内容分页
        /// <summary>
        /// 长文章分页
        /// </summary>
        /// <param name="content">长文章内容</param>
        /// <param name="url">路径</param>
        /// <param name="page">分页传参</param>
        /// <param name="s">分隔符</param>
        /// <returns></returns>
        public static string Compage(string content, string url, string page, string s)
        {
            var tempstr = string.Empty;
            if (content.IndexOf(s, StringComparison.Ordinal) > -1)
            {
                var pp = string.Empty;
                if (Current.Request.QueryString.ToString().IndexOf(page, StringComparison.Ordinal) > -1)
                {
                    pp = Q(page).ToString();
                }
                var p = 1;
                if (pp.Equals("") || !IsNumeric(pp) || int.Parse(pp) <= 0)
                {
                    p = 1;
                }
                else
                {
                    p = int.Parse(pp);
                }
                var c = content.Replace(s, "＄").Split('＄');
                var j = 0;
                var maxpage = c.Length;
                var endpage = 0;
                if (p >= maxpage)
                {
                    p = maxpage;
                }
                tempstr += "<div style=\"height:90%;line-height:18px;padding-bottom:15px\">" + c[p - 1] + "</div><div style=\"text-align:center\">";
                if (p <= 1)
                {
                    tempstr += "上一页";
                }
                else
                {
                    tempstr += "<a href=\"" + url + page + "=" + (p - 1) + "\">上一页</a>";
                }
                tempstr += "&nbsp;第<b>";
                if (p > 4)
                {
                    tempstr += "<a href=\"" + url + page + "=1\">1</a>…";
                }
                if (maxpage > p + 3)
                {
                    endpage = p + 3;
                }
                else
                {
                    endpage = maxpage + 1;
                }
                for (j = p - 3; j <= endpage; j++)
                {
                    if (j <= 1) continue;
                    if (j == (p + 1))
                    {
                        tempstr += "<span style=\"color:#ff0000;padding:0px 3px;font-size:16px\">" + (j - 1) + "</span>";
                    }
                    else
                    {
                        tempstr += "<span style=\"padding:0px 3px\"><a href=\"" + url + page + "=" + (j - 1) + "\">" + (j - 1) + "</a></span>";
                    }
                }
                if (p + 3 < maxpage)
                {
                    tempstr += "…<a href=\"" + url + page + "=" + (maxpage + 1) + "\"><b>" + (maxpage + 1) + "</b></a>";
                }
                tempstr += "</b>页&nbsp;";
                if (p + 3 >= maxpage + 4)
                {
                    tempstr += "下一页";
                }
                else
                {
                    tempstr += "<a href=\"" + url + page + "=" + (p + 1) + "\">下一页</a>";
                }
                tempstr += "</div>";
            }
            else
            {
                tempstr += content;
            }
            return tempstr;
        }
        #endregion

        #region 邮件处理
        /// <summary>
        /// 群发邮件或单条发送
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="emailfrom">发件人邮箱</param>
        /// <param name="emailto">收件人邮箱</param>
        /// <param name="smtpserver">SMTP服务器</param>
        /// <param name="user">用户名</param>
        /// <param name="pass">密码</param>
        public static void SendEmail(string subject, string body, string emailfrom, string emailto, string smtpserver, string user, string pass)
        {

            var emailSubject = subject.ToString();
            var emailBody = body.ToString();
            var fromEmail = emailfrom.ToString().Trim();  //你的email
            var toEmai = emailto.ToString();　//对方的email 
            toEmai = toEmai.Replace("\n", "").Replace(" ", "");
            if (toEmai.IndexOf(";", StringComparison.Ordinal) > -1)
            {
                var str = toEmai.Split(';');

                foreach (var t in str)
                {
                    DoSend(emailSubject, emailBody, emailfrom, t, smtpserver, user, pass);
                }
            }
            else
            {
                DoSend(emailSubject, emailBody, emailfrom, toEmai, smtpserver, user, pass);
            }
        }

        /// <summary>
        /// 处理发邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="emailfrom">发件人邮箱</param>
        /// <param name="emailto">收件人邮箱</param>
        /// <param name="smtpserver">SMTP服务器</param>
        /// <param name="user">用户名</param>
        /// <param name="pass">密码</param>
        private static void DoSend(string subject, string body, string emailfrom, string emailto, string smtpserver, string user, string pass)
        {
            var t = DateTime.Now;
            var mail = new MailMessage(emailfrom, emailto)
            {
                Subject = subject,
                SubjectEncoding = Encoding.GetEncoding("utf-8"),
                Body = body + t.ToString(CultureInfo.InvariantCulture),
                IsBodyHtml = true,
                Priority = MailPriority.High
            };
            //发送邮件
            var smtp = new SmtpClient
            {
                Host = smtpserver,
                Credentials = new NetworkCredential(user, pass)
            };
            //smtp.163.com
            smtp.Send(mail);
        }
        #endregion

        #region 样式处理
        /// <summary>
        /// 字体样式转化函数
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string FontStyle(int flag)
        {
            string tempstr;
            switch (flag)
            {
                case 1:
                    tempstr = "font-weight:bold;";
                    break;
                case 2:
                    tempstr = "font-style:italic;";
                    break;
                case 3:
                    tempstr = "font-weight:bold;font-style:italic;";
                    break;
                default:
                    tempstr = "";
                    break;
            }
            return tempstr;
        }
        /// <summary>
        /// 获取临时变量传参
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetId(string tid, string val)
        {
            var pid = val;
            if (Current.Request.QueryString.ToString().IndexOf(tid, StringComparison.Ordinal) <= -1) return pid;
            if (Current.Request.QueryString[tid] == null) return pid;
            var tempstr = Current.Request.QueryString[tid].ToString();
            if (!string.IsNullOrEmpty(tempstr))
            {
                pid = tempstr;
            }
            return pid;
        }
        /// <summary>
        /// 获取选中样式
        /// </summary>
        /// <param name="geturl"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetStyle(string geturl, string val)
        {
            var url = Current.Request.RawUrl.Substring(Current.Request.RawUrl.LastIndexOf("/", StringComparison.Ordinal) + 1);
            var tempstr = geturl.IndexOf(url, StringComparison.Ordinal) > -1 ? val : "";
            return tempstr;
        }
        #endregion

        #region 判断验证
        /// <summary>
        /// 判断验证函数
        /// </summary>
        /// <param name="v"></param>
        /// <param name="p"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string Validate(string v, string p, string e)
        {
            var tempstr = string.Empty;
            var reg = new Regex(p);
            if (reg.IsMatch(v) == false)
            {
                tempstr = e;
            }
            return tempstr;
        }
        /// <summary>
        /// 判断是否匹配
        /// </summary>
        /// <param name="v"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsVal(string v, string p)
        {
            return new Regex(p).IsMatch(v);
        }
        #endregion

        #region 图片处理
        /// <summary>
        /// 远程保存图片
        /// </summary>
        /// <param name="pic">图片路径</param>
        /// <param name="filePath">保存路径</param>
        /// <param name="newName">保存新文件名</param>
        public static void RemotePic(string pic, string filePath, string newName)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var client = new WebClient();
            client.DownloadFile(pic, filePath + "/" + newName);
        }
        #endregion

        #region 编码转换
        /// <summary>    
        /// 写入时进行转换    
        /// </summary>    
        /// <param name="write">要写入的字符串</param>    
        /// <returns></returns>    
        public static string ToIso(string write)
        {
            //声明字符集        
            //iso8859        
            var iso8859 = Encoding.GetEncoding("iso8859-1");
            //国标2312        
            var gb2312 = Encoding.GetEncoding("gb2312");
            var gb = gb2312.GetBytes(write);
            //返回转换后的字符        
            return iso8859.GetString(gb);
        }
        /// <summary>
        /// 读出时进行转换 
        /// </summary>
        /// <param name="read"></param>
        /// <returns></returns>
        public static string ToGBK(string read)
        {
            //声明字符集        
            //iso8859        
            var iso8859 = Encoding.GetEncoding("iso8859-1");
            //国标2312        
            var gb2312 = Encoding.GetEncoding("gb2312");
            var iso = iso8859.GetBytes(read);
            //返回转换后的字符        
            return gb2312.GetString(iso);
        }
        /// <summary>
        /// 批量数据转换，将dataset的内容读出到xml文件，然后再输出  
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataSet ToGBK(DataSet ds)
        {
            var xml = ds.GetXml();
            ds.Clear();
            //声明字符集        
            //iso8859        
            var iso8859 = Encoding.GetEncoding("iso8859-1");
            //国标2312        
            var gb2312 = Encoding.GetEncoding("gb2312");
            var bt = iso8859.GetBytes(xml);
            xml = gb2312.GetString(bt);
            ds.ReadXml(new StringReader(xml));
            return ds;
        }
        #endregion

        #region MySQL设置编码
        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string MySqlSetCharcter(string input)
        {
            //  string s = Encoding.GetEncoding("GBK").GetString(Encoding.Default.GetBytes(input));                        
            var bt = Encoding.UTF8.GetBytes(input);
            var ec = Encoding.GetEncoding("GB2312");
            // ec = Encoding.GetEncoding("GBK");        
            var flagtxt = IsGbkCode(ec.GetString(bt));

            return ec.GetString(bt);
        }
        /// <summary>         
        /// 判断一个Stringutf8是否为GBK编码的汉字      
        /// </summary>         
        /// <param name="stringutf8"></param>       
        /// <returns></returns>       
        private static bool IsGbkCode(string stringutf8)
        {
            var bytes = Encoding.GetEncoding("GBK").GetBytes(stringutf8.ToString());
            if (bytes.Length <= 1)
            // if there is only one byte, it is ASCII code           
            {
                return false;
            }
            else
            {
                var byte1 = bytes[0];
                var byte2 = bytes[1];
                return byte1 >= 129 && byte1 <= 254 && byte2 >= 64 && byte2 <= 254;
            }
        }
        #endregion MySQL设置编码


    }
}
