using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DoIt
{
    /// <summary>
    /// 验证字符串是否合法
    /// </summary>
    public class ValidateUtil
    {

        #region 构造函数
        /// <summary>构造函数</summary>
        public ValidateUtil()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            // 
        }
        #endregion

        #region 验证字符处理类

        /// <summary>是否空</summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>true/false</returns>
        public static bool IsBlank(string strInput)
        {
            return string.IsNullOrEmpty(strInput);
        }

        /// <summary>是否数字</summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>true/false</returns>
        public static bool IsNumeric(string strInput)
        {

            char[] ca = strInput.ToCharArray();
            bool found = true;
            for (int i = 0; i < ca.Length; i++)
            {
                if ((ca[i] < '0' || ca[i] > '9') && ca[i] != '.')
                {
                    found = false;
                    break;
                }
            }
            return found;
        }

        /// <summary>是否日期</summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>true/false</returns>
        public static bool IsDate(string strInput)
        {
            string datestr = strInput;
            string year, month, day;
            string[] c ={ "/", "-", "." };
            string cs = string.Empty;
            for (int i = 0; i < c.Length; i++)
            {
                if (datestr.IndexOf(c[i]) > 0)
                {
                    cs = c[i];
                    break;
                }
            }

            if (string.IsNullOrEmpty(cs))
            {
                year = datestr.Substring(0, datestr.IndexOf(cs));
                if (year.Length != 4) { return false; };
                datestr = datestr.Substring(datestr.IndexOf(cs) + 1);

                month = datestr.Substring(0, datestr.IndexOf(cs));
                if ((month.Length != 2) || (Convert.ToInt16(month) > 12))
                { return false; };
                datestr = datestr.Substring(datestr.IndexOf(cs) + 1);

                day = datestr;
                if ((day.Length != 2) || (Convert.ToInt16(day) > 31)) { return false; };

                return CheckDatePart(year, month, day);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查年月日是否合法
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private static bool CheckDatePart(string year, string month, string day)
        {
            int iyear = int.Parse(year);
            int imonth = int.Parse(month);
            int iday = int.Parse(day);
            if (iyear > 2099 || iyear < 1900) { return false; }
            if (imonth > 12 || imonth < 1) { return false; }
            if (iday > DateUtil.GetDaysOfMonth(iyear, imonth) || iday < 1) { return false; };
            return true;
        }

        /// <summary>是否Null</summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>true/false</returns>
        public static bool IsNull(object strInput)
        {
            return strInput == null ? true : false;
        }

        /// <summary>是否为Double</summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>true/false</returns>
        public static bool IsDouble(string strInput)
        {
            Regex reg = new Regex("^-?([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0)$", RegexOptions.IgnoreCase);
            return reg.IsMatch(strInput);
        }

        /// <summary>是否为Int</summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>true/false</returns>
        public static bool IsInt(string strInput)
        {
            Regex reg = new Regex("^-?[1-9]\\d*$", RegexOptions.IgnoreCase);
            return reg.IsMatch(strInput); 
        }

        /// <summary>是否为合法的电话号码</summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>true/false</returns>
        public static bool IsTel(string strInput)
        {
            Regex reg = new Regex("^(([0\\+]\\d{2,3}-)?(0\\d{2,3})-)?(\\d{7,8})(-(\\d{3,}))?$", RegexOptions.IgnoreCase);
            return reg.IsMatch(strInput);
        }

        /// <summary>是否为合法的邮政编码</summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>true/false</returns>
        public static bool IsZip(string strInput)
        {
            Regex reg = new Regex("^\\d{6}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(strInput);
        }

        /// <summary>是否为合法的电子邮件</summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>true/false</returns>
        public static bool IsEmail(string strInput)
        {
            Regex reg = new Regex("^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", RegexOptions.IgnoreCase);
            return reg.IsMatch(strInput);
        }

        /// <summary>
        /// 是否为合法的身份证号
        /// </summary>
        /// <param name="cid">身份证号码</param>
        /// <returns>true/false</returns>
        public static bool IsCardId(string cid)
        {
            if (cid.Length == 15)
            {
                cid = Per15To18(cid);
            }
            string[] aCity = new string[] { null, null, null, null, null, null, null, null, null, null, null, "北京", "天津", "河北", "山西", "内蒙古", null, null, null, null, null, "辽宁", "吉林", "黑龙江", null, null, null, null, null, null, null, "上海", "江苏", "浙江", "安微", "福建", "江西", "山东", null, null, null, "河南", "湖北", "湖南", "广东", "广西", "海南", null, null, null, "重庆", "四川", "贵州", "云南", "西藏", null, null, null, null, null, null, "陕西", "甘肃", "青海", "宁夏", "新疆", null, null, null, null, null, "台湾", null, null, null, null, null, null, null, null, null, "香港", "澳门", null, null, null, null, null, null, null, null, "国外" };
            double iSum = 0;
            //string info=""; 
            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|x)$");
            System.Text.RegularExpressions.Match mc = rg.Match(cid);
            if (!mc.Success)
            {
                // return "格式不正确!"; 
                return false;
            }
            cid = cid.ToLower();
            cid = cid.Replace("x", "a");
            if (aCity[int.Parse(cid.Substring(0, 2))] == null)
            {
                //return "非法地区"; 
                return false;
            }
            try
            {
                DateTime.Parse(cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2));
            }
            catch
            {
                //return "非法生日"; 
                return false;
            }
            for (int i = 17; i >= 0; i--)
            {
                iSum += (System.Math.Pow(2, i) % 11) * int.Parse(cid[17 - i].ToString(), System.Globalization.NumberStyles.HexNumber);

            }
            if (iSum % 11 != 1)
                //return("非法证号"); 
                return false;

            //return (cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2));
            return true;

        }

        /// <summary>
        /// 身份证号码升位 15-18 算法 15位返回18位号码
        /// </summary>
        /// <param name="perIdSrc">15位的身份证号码</param>
        /// <returns>18位的身份证号码</returns>
        private static string Per15To18(string perIdSrc)
        {
            int iS = 0;

            //加权因子常数 
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //校验码常数 
            string lastCode = "10X98765432";
            //新身份证号 
            string perIdNew;

            perIdNew = perIdSrc.Substring(0, 6);
            //填在第6位及第7位上填上‘1’，‘9’两个数字 
            perIdNew += "19";
            perIdNew += perIdSrc.Substring(6, 9);
            //进行加权求和 
            for (int i = 0; i < 17; i++)
            {
                iS += int.Parse(perIdNew.Substring(i, 1)) * iW[i];
            }

            //取模运算，得到模值 
            int iY = iS % 11;
            //从LastCode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。 
            perIdNew += lastCode.Substring(iY, 1);

            return perIdNew;
        }
        #endregion

    }
}
