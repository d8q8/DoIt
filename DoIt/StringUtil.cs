using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DoIt
{
    /// <summary>
    /// 字符串操作工具集
    /// </summary>
    public class StringUtil
    {

        #region 构造函数
        public StringUtil()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //            
        }
        #endregion

        #region 字符处理类
        /// <summary>
        /// 从字符串中的尾部删除指定的字符串
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="removedString"></param>
        /// <returns></returns>
        public static string Remove(string sourceString, string removedString)
        {
            try
            {
                if (sourceString.IndexOf(removedString) < 0)
                    throw new Exception("原字符串中不包含移除字符串！");
                string result = sourceString;
                int lengthOfSourceString = sourceString.Length;
                int lengthOfRemovedString = removedString.Length;
                int startIndex = lengthOfSourceString - lengthOfRemovedString;
                string tempSubString = sourceString.Substring(startIndex);
                if (tempSubString.ToUpper() == removedString.ToUpper())
                {
                    result = sourceString.Remove(startIndex, lengthOfRemovedString);
                }
                return result;
            }
            catch
            {
                return sourceString;
            }
        }

        /// <summary>
        /// 获取拆分符右边的字符串
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string RightSplit(string sourceString, char splitChar)
        {
            string result = null;
            string[] tempString = sourceString.Split(splitChar);
            if (tempString.Length > 0)
            {
                result = tempString[tempString.Length - 1].ToString();
            }
            return result;
        }

        /// <summary>
        /// 获取拆分符左边的字符串
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string LeftSplit(string sourceString, char splitChar)
        {
            string result = null;
            string[] tempString = sourceString.Split(splitChar);
            if (tempString.Length > 0)
            {
                result = tempString[0].ToString();
            }
            return result;
        }

        /// <summary>
        /// 去掉最后一个逗号
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static string DelLastComma(string origin)
        {
            if (origin.IndexOf(",") == -1)
            {
                return origin;
            }
            return origin.Substring(0, origin.LastIndexOf(","));
        }

        /// <summary>
        /// 删除不可见字符
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string DeleteUnVisibleChar(string sourceString)
        {
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder(131);
            for (int i = 0; i < sourceString.Length; i++)
            {
                int unicode = sourceString[i];
                if (unicode >= 16)
                {
                    sBuilder.Append(sourceString[i].ToString());
                }
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 获取数组元素的合并字符串
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public static string GetArrayString(string[] stringArray)
        {
            string totalString = null;
            for (int i = 0; i < stringArray.Length; i++)
            {
                totalString = totalString + stringArray[i];
            }
            return totalString;
        }

        /// <summary>
        /// 获取某一字符串在字符串数组中出现的次数
        /// </summary>
        /// <param name="stringArray"></param>
        /// <param name="findString"></param>
        /// <returns></returns>
        public static int GetStringCount(string[] stringArray, string findString)
        {
            int count = -1;
            string totalString = GetArrayString(stringArray);
            string subString = totalString;

            while (subString.IndexOf(findString) >= 0)
            {
                subString = totalString.Substring(subString.IndexOf(findString));
                count += 1;
            }
            return count;
        }

        /// <summary>
        /// 获取某一字符串在字符串中出现的次数
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="findString"></param>
        /// <returns></returns>
        public static int GetStringCount(string sourceString, string findString)
        {
            int count = 0;
            int findStringLength = findString.Length;
            string subString = sourceString;

            while (subString.IndexOf(findString) >= 0)
            {
                subString = subString.Substring(subString.IndexOf(findString) + findStringLength);
                count += 1;
            }
            return count;
        }

        /// <summary>
        /// 截取从startString开始到原字符串结尾的所有字符
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="startString"></param>
        /// <returns></returns>
        public static string GetSubString(string sourceString, string startString)
        {
            try
            {
                int index = sourceString.ToUpper().IndexOf(startString);
                if (index > 0)
                {
                    return sourceString.Substring(index);
                }
                return sourceString;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 截取从startString开始到原字符串结尾的所有字符
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="beginRemovedString"></param>
        /// <param name="endRemovedString"></param>
        /// <returns></returns>
        public static string GetSubString(string sourceString, string beginRemovedString, string endRemovedString)
        {
            try
            {
                if (sourceString.IndexOf(beginRemovedString) != 0)
                    beginRemovedString = "";

                if (sourceString.LastIndexOf(endRemovedString, sourceString.Length - endRemovedString.Length) < 0)
                    endRemovedString = "";

                int startIndex = beginRemovedString.Length;
                int length = sourceString.Length - beginRemovedString.Length - endRemovedString.Length;
                if (length > 0)
                {
                    return sourceString.Substring(startIndex, length);
                }
                return sourceString;
            }
            catch
            {
                return sourceString; ;
            }
        }

        /// <summary>
        /// 按字节数取出字符串的长度
        /// </summary>
        /// <param name="strTmp">要计算的字符串</param>
        /// <returns>字符串的字节数</returns>
        public static int GetByteCount(string strTmp)
        {
            int intCharCount = 0;
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (System.Text.UTF8Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    intCharCount = intCharCount + 2;
                }
                else
                {
                    intCharCount = intCharCount + 1;
                }
            }
            return intCharCount;
        }

        /// <summary>
        /// 按字节数要在字符串的位置
        /// </summary>
        /// <param name="intIns">字符串的位置</param>
        /// <param name="strTmp">要计算的字符串</param>
        /// <returns>字节的位置</returns>
        public static int GetByteIndex(int intIns, string strTmp)
        {
            int intReIns = 0;
            if (strTmp.Trim() == "")
            {
                return intIns;
            }
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (System.Text.UTF8Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    intReIns = intReIns + 2;
                }
                else
                {
                    intReIns = intReIns + 1;
                }
                if (intReIns >= intIns)
                {
                    intReIns = i + 1;
                    break;
                }
            }
            return intReIns;
        }

        #region 替换字符串
        /// <summary> 
        /// 功能:替换字符 
        /// </summary> 
        /// <param name="strVAlue">字符串</param> 
        /// <returns>替换掉'的字符串</returns> 
        public static string FilterSql(string strVAlue)
        {
            string str = "";
            str = strVAlue.Replace("''", "");
            return str;
        }
        #endregion

        #region 对表 表单内容进行转换HTML操作,
        /// <summary> 
        /// 功能:对表 表单内容进行转换HTML操作, 
        /// </summary> 
        /// <param name="fString">html字符串</param> 
        /// <returns></returns> 
        public static string HtmlCode(string fString)
        {
            string str = "";
            str = fString.Replace(">", ">");
            str = fString.Replace("<", "<");
            str = fString.Replace(" ", " ");
            str = fString.Replace("\n", "<br />");
            str = fString.Replace("\r", "<br />");
            str = fString.Replace("\r\n", "<br />");
            return str;
        }
        #endregion

        #region 判断是否:返回值：√ or ×
        /// <summary> 
        /// 判断是否:返回值：√ or × 
        /// </summary> 
        /// <param name="b">true 或false</param> 
        /// <returns>√ or ×</returns> 
        public static string Judgement(bool b)
        {
            string s = "";
            if (b == true)
                s = "<b><font color=\"#009900\">√</font></b>";
            else
                s = "<b><font color=\"#FF0000\">×</font></b>";
            return s;
        }
        #endregion

        #region 截取字符串
        /// <summary> 
        /// 功能:截取字符串长度 
        /// </summary> 
        /// <param name="str">要截取的字符串</param> 
        /// <param name="length">字符串长度</param> 
        /// <param name="flg">true:加...,flase:不加</param> 
        /// <returns></returns> 
        public static string GetString(string str, int length, bool flg)
        {
            int i = 0, j = 0;
            foreach (char chr in str)
            {
                if ((int)chr > 127)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }
                if (i > length)
                {
                    str = str.Substring(0, j);
                    if (flg)
                        str += "......";
                    break;
                }
                j++;
            }
            return str;
        }
        #endregion

        #region 截取字符串+…
        /// <summary> 
        /// 截取字符串+… 
        /// </summary> 
        /// <param name="strInput"></param> 
        /// <param name="intlen"></param> 
        /// <returns></returns> 
        public static string CutString(string strInput, int intlen)//截取字符串 
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int intLength = 0;
            string strString = "";
            byte[] s = ascii.GetBytes(strInput);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    intLength += 2;
                }
                else
                {
                    intLength += 1;
                }
                try
                {
                    strString += strInput.Substring(i, 1);
                }
                catch
                {
                    break;
                }
                if (intLength > intlen)
                {
                    break;
                }
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(strInput);
            if (mybyte.Length > intlen)
            {
                strString += "…";
            }
            return strString;
        }
        #endregion

        #region 字符串分函数
        /// <summary> 
        /// 字符串分函数 
        /// </summary> 
        /// <param name="strID"></param> 
        /// <param name="index"></param> 
        /// <param name="separ"></param> 
        /// <returns></returns> 
        public string StringSplit(string strings, int index, string separ)
        {
            string[] s = strings.Split(char.Parse(separ));
            return s[index];
        }
        #endregion

        #region 分解字符串为数组
        /// <summary> 
        /// 字符串分函数 
        /// </summary> 
        /// <param name="str">要分解的字符串</param> 
        /// <param name="splitstr">分割符,可以为string类型</param> 
        /// <returns>字符数组</returns> 
        public static string[] Splitstr(string str, string splitstr)
        {
            if (splitstr != "")
            {
                System.Collections.ArrayList c = new System.Collections.ArrayList();
                while (true)
                {
                    int thissplitindex = str.IndexOf(splitstr);
                    if (thissplitindex >= 0)
                    {
                        c.Add(str.Substring(0, thissplitindex));
                        str = str.Substring(thissplitindex + splitstr.Length);
                    }
                    else
                    {
                        c.Add(str);
                        break;
                    }
                }
                string[] d = new string[c.Count];
                for (int i = 0; i < c.Count; i++)
                {
                    d[i] = c[i].ToString();
                }
                return d;
            }
            else
            {
                return new string[] { str };
            }
        }
        #endregion

        #region URL编码
        /// <summary> 
        /// URL编码 
        /// </summary> 
        /// <param name="str">字符串</param> 
        /// <returns></returns> 
        public static string UrlEncoding(string str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            return System.Text.Encoding.UTF8.GetString(bytes).ToString();
        }
        #endregion

        #region 获取Web.config中的配置字段值
        /// <summary> 
        /// 获取全局配置参数 
        /// </summary> 
        /// <param name="key">键名</param> 
        /// <returns>参数</returns> 
        public static string GetApp(string key)
        {
            System.Configuration.AppSettingsReader appr = new System.Configuration.AppSettingsReader();
            try
            {
                string str = (string)appr.GetValue(key, typeof(string));
                if (str == null || str == "") return null;
                return str;
            }
            catch { }
            return null;
        }
        #endregion

        #region 根据传入的字符串是否为yes/no返回Bit
        /// <summary> 
        /// 根据传入的字符串是否为yes/no返回Bit 
        /// </summary> 
        /// <param name="flg"></param> 
        /// <returns></returns> 
        public static int GetBitBool(string flg)
        {
            int str = 0;
            switch (flg.ToLower())
            {
                case "yes":
                    str = 1;
                    break;
                case "no":
                    str = 0;
                    break;
                default:
                    break;
            }
            return str;
        }
        #endregion

        #region Html编码
        /// <summary> 
        /// HTML编码 
        /// </summary> 
        /// <param name="strInput"></param> 
        /// <returns></returns> 
        public static string HtmlEncode(string strInput)
        {
            string str;
            try
            {
                str = HttpContext.Current.Server.HtmlEncode(strInput);
            }
            catch
            {
                str = "error";
            }
            return str;
        }
        /// <summary> 
        /// HTML解码 
        /// </summary> 
        /// <param name="strInput"></param> 
        /// <returns></returns> 
        public static string HtmlDecode(string strInput)
        {
            string str;
            try
            {
                str = HttpContext.Current.Server.HtmlDecode(strInput);
            }
            catch
            {
                str = "error";
            }
            return str;
        }
        #endregion

        #region 检测一个字符符,是否在另一个字符中,存在,存在返回true,否则返回false
        /// <summary> 
        /// 检测一个字符符,是否在另一个字符中,存在,存在返回true,否则返回false 
        /// </summary> 
        /// <param name="srcString">原始字符串</param> 
        /// <param name="aimString">目标字符串</param> 
        /// <returns></returns> 
        public static bool IsEnglish(string srcString, string aimString)
        {
            bool rev = true;
            string chr;
            if (aimString == "" || aimString == null) return false;
            for (int i = 0; i < aimString.Length; i++)
            {
                chr = aimString.Substring(i, 1);
                if (srcString.IndexOf(chr) < 0)
                {
                    return false;
                }
            }
            return rev;
        }
        #endregion

        #region 检测字符串中是否含有中文及中文长度
        /// <summary> 
        /// 检测字符串中是否含有中文及中文长度 
        /// </summary> 
        /// <param name="str">要检测的字符串</param> 
        /// <returns>中文字符串长度</returns> 
        public static int CnStringLength(string str)
        {
            ASCIIEncoding n = new ASCIIEncoding();
            byte[] b = n.GetBytes(str);
            int l = 0; // l 为字符串之实际长度 
            for (int i = 0; i <= b.Length - 1; i++)
            {
                if (b[i] == 63) //判断是否为汉字或全脚符号 
                {
                    l++;
                }
            }
            return l;
        }
        #endregion

        #region 取字符串右侧的几个字符
        /// <summary> 
        /// 取字符串右侧的几个字符 
        /// </summary> 
        /// <param name="str">字符串</param> 
        /// <param name="length">右侧的几个字符</param> 
        /// <returns></returns> 
        public static string GetStrRight(string str, int length)
        {
            string rev = "";
            if (str.Length < length)
            {
                rev = str;
            }
            else
            {
                rev = str.Substring(str.Length - length, length);
            }
            return rev;
        }
        #endregion

        #region 替换右侧的字符串
        /// <summary> 
        /// 替换右侧的字符串 
        /// </summary> 
        /// <param name="str">字符串</param> 
        /// <param name="strsrc">右侧的字符串</param> 
        /// <param name="straim">要替换为的字符串</param> 
        /// <returns></returns> 
        public static string RepStrRight(string str, string strsrc, string straim)
        {
            string rev = "";
            if (GetStrRight(str, strsrc.Length) != strsrc)
            {
                rev = str;
            }
            else
            {
                rev = str.Substring(0, str.Length - strsrc.Length).ToString() + straim.ToString();
            }
            return rev;
        }
        #endregion

        #endregion

    }
}
