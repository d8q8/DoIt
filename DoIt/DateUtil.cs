using System;
using System.Collections.Generic;
using System.Text;

namespace DoIt
{
    /// <summary>
    /// 日期处理函数包
    /// </summary>
    public static class DateUtil
    {

        #region 调用方法
        /*=======================================================================================
         * 使用方法：
         *      //按年返回本年的天数
         *      int yeardays = DateUtil.GetDaysOfYear(2008);
         *      //按当前日期返回本年的天数
         *      int datedays = DateUtil.GetDaysOfYear(DateTime.Parse('2008-5-1'));
         *      //按年月返回月份天数
         *      int monthdays = DateUtil.GetDaysOfMonth(2008,5);
         *      //按当前日期返回月份天数
         *      int monthdays = DateUtil.GetDaysOfMonth(DateTime.Parse("2008-5-1"));
         *      //按当前日期返回星期名称
         *      string weekname = DateUtil.GetWeekNameOfDay(DateTime.Parse("2008-5-1"));
         *      //按当前日期返回星期编号
         *      string weeknum = DateUtil.GetWeekNumberOfDay(DateTime.Parse("2008-5-1"));
         *      //判断当前日期年份是否为闰年
         *      bool runyear = DateUtil.IsRuYear(DateTime.Parse("2008-5-1"));
         *      //判断当前年份是否为闰年
         *      bool runyear = DateUtil.IsRuYear(2008);
         *      //将字符串转化为日期型
         *      DateTime dt = DateUtil.ConvertStringToDate("2008-5-1");
         *      //将日期转化为字符串
         *      string dt = DateUtil.ConvertDateToString(DateTime.Parse("2008-5-1"),"yyyy");
         *      //判断是否为日期型
         *      bool isdate = DateUtil.IsDateTime("2008-5-1");
         *      //日期差计算
         *      double diff = DateUtil.DateDiff("year",DateTime.Parse("2008-1-1"),DateTime.Parse("2009-1-1"));
        =======================================================================================*/
        #endregion

        #region 日期方法处理

        /// <summary>返回本年有多少天</summary>
        /// <param name="iYear">年份</param>
        /// <returns>本年的天数</returns>
        public static int GetDaysOfYear(int iYear)
        {
            return IsRuYear(iYear) ? 366 : 365;
        }

        /// <summary>本年有多少天</summary>
        /// <param name="idt">日期</param>
        /// <returns>本天在当年的天数</returns>
        public static int GetDaysOfYear(DateTime idt)
        {
            //取得传入参数的年份部分，用来判断是否是闰年
            return IsRuYear(idt.Year) ? 366 : 365;

        }

        /// <summary>本月有多少天</summary>
        /// <param name="iYear">年</param>
        /// <param name="month">月</param>
        /// <returns>天数</returns>
        public static int GetDaysOfMonth(int iYear, int month)
        {
            var days = 0;
            switch (month)
            {
                case 1:
                    days = 31;
                    break;
                case 2:
                    days = IsRuYear(iYear) ? 29 : 28;
                    break;
                case 3:
                    days = 31;
                    break;
                case 4:
                    days = 30;
                    break;
                case 5:
                    days = 31;
                    break;
                case 6:
                    days = 30;
                    break;
                case 7:
                    days = 31;
                    break;
                case 8:
                    days = 31;
                    break;
                case 9:
                    days = 30;
                    break;
                case 10:
                    days = 31;
                    break;
                case 11:
                    days = 30;
                    break;
                case 12:
                    days = 31;
                    break;
            }
            return days;
        }

        /// <summary>本月有多少天</summary>
        /// <param name="dt">日期</param>
        /// <returns>天数</returns>
        public static int GetDaysOfMonth(DateTime dt)
        {
            //--------------------------------//
            //--从dt中取得当前的年，月信息  --//
            //--------------------------------//
            var days = 0;
            var year = dt.Year;
            var month = dt.Month;

            //--利用年月信息，得到当前月的天数信息。
            switch (month)
            {
                case 1:
                    days = 31;
                    break;
                case 2:
                    days = IsRuYear(year) ? 29 : 28;
                    break;
                case 3:
                    days = 31;
                    break;
                case 4:
                    days = 30;
                    break;
                case 5:
                    days = 31;
                    break;
                case 6:
                    days = 30;
                    break;
                case 7:
                    days = 31;
                    break;
                case 8:
                    days = 31;
                    break;
                case 9:
                    days = 30;
                    break;
                case 10:
                    days = 31;
                    break;
                case 11:
                    days = 30;
                    break;
                case 12:
                    days = 31;
                    break;
            }
            return days;
        }

        /// <summary>返回当前日期的星期名称</summary>
        /// <param name="dt">日期</param>
        /// <returns>星期名称</returns>
        public static string GetWeekNameOfDay(DateTime idt)
        {
            var week = "";
            var dt = idt.DayOfWeek.ToString();
            switch (dt)
            {
                case "Mondy":
                    week = "星期一";
                    break;
                case "Tuesday":
                    week = "星期二";
                    break;
                case "Wednesday":
                    week = "星期三";
                    break;
                case "Thursday":
                    week = "星期四";
                    break;
                case "Friday":
                    week = "星期五";
                    break;
                case "Saturday":
                    week = "星期六";
                    break;
                case "Sunday":
                    week = "星期日";
                    break;
            }
            return week;
        }

        /// <summary>返回当前日期的星期编号</summary>
        /// <param name="idt">日期</param>
        /// <returns>星期数字编号</returns>
        public static string GetWeekNumberOfDay(DateTime idt)
        {
            var week = "";
            var dt = idt.DayOfWeek.ToString();
            switch (dt)
            {
                case "Mondy":
                    week = "1";
                    break;
                case "Tuesday":
                    week = "2";
                    break;
                case "Wednesday":
                    week = "3";
                    break;
                case "Thursday":
                    week = "4";
                    break;
                case "Friday":
                    week = "5";
                    break;
                case "Saturday":
                    week = "6";
                    break;
                case "Sunday":
                    week = "7";
                    break;
            }
            return week;
        }

        /// <summary>判断当前日期所属的年份是否是闰年，私有函数</summary>
        /// <param name="idt">日期</param>
        /// <returns>是闰年：True ，不是闰年：False</returns>
        private static bool IsRuYear(DateTime idt)
        {
            //形式参数为日期类型 
            //例如：2003-12-12
            var n = idt.Year;
            return (n % 400 == 0) || (n % 4 == 0 && n % 100 != 0);
        }

        /// <summary>判断当前年份是否是闰年，私有函数</summary>
        /// <param name="iYear">年份</param>
        /// <returns>是闰年：True ，不是闰年：False</returns>
        private static bool IsRuYear(int iYear)
        {
            //形式参数为年份
            //例如：2003
            var n = iYear;
            return (n % 400 == 0) || (n % 4 == 0 && n % 100 != 0);
        }

        /// <summary>
        /// 将输入的字符串转化为日期。如果字符串的格式非法，则返回当前日期。
        /// </summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>日期对象</returns>
        public static DateTime ConvertStringToDate(string strInput)
        {
            DateTime oDateTime;
            try
            {
                oDateTime = DateTime.Parse(strInput);
            }
            catch (Exception)
            {
                oDateTime = DateTime.Today;
            }
            return oDateTime;
        }

        /// <summary>
        /// 将日期对象转化为格式字符串
        /// </summary>
        /// <param name="oDateTime">日期对象</param>
        /// <param name="strFormat">
        /// 格式：
        ///        "SHORTDATE"===短日期
        ///        "LONGDATE"==长日期
        ///        其它====自定义格式
        /// </param>
        /// <returns>日期字符串</returns>
        public static string ConvertDateToString(DateTime oDateTime, string strFormat)
        {
            var strDate = "";
            try
            {
                switch (strFormat.ToUpper())
                {
                    case "SHORTDATE":
                        strDate = oDateTime.ToShortDateString();
                        break;
                    case "LONGDATE":
                        strDate = oDateTime.ToLongDateString();
                        break;
                    default:
                        strDate = oDateTime.ToString(strFormat);
                        break;
                }
            }
            catch (Exception)
            {
                strDate = oDateTime.ToShortDateString();
            }

            return strDate;
        }

        /// <summary>
        /// 判断是否为合法日期，必须大于1800年1月1日
        /// </summary>
        /// <param name="strDate">输入日期字符串</param>
        /// <returns>True/False</returns>
        public static bool IsDateTime(string strDate)
        {
            try
            {
                var oDate = DateTime.Parse(strDate);
                return oDate.CompareTo(DateTime.Parse("1800-1-1")) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取两个日期之间的差值,获取年数使用 (int)DateDiff(string howtocompare, DateTime startDate, DateTime endDate)
        /// </summary>
        /// <param name="howtocompare">比较的方式：year month day hour minute second</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>时间差</returns>
        public static double DateDiff(string howtocompare, DateTime startDate, DateTime endDate)
        {
            double diff = 0;
            try
            {
                var ts = new TimeSpan(endDate.Ticks - startDate.Ticks);

                switch (howtocompare.ToLower())
                {
                    case "year":
                        diff = Convert.ToDouble(ts.TotalDays / 365);
                        break;
                    case "month":
                        diff = Convert.ToDouble((ts.TotalDays / 365) * 12);
                        break;
                    case "day":
                        diff = Convert.ToDouble(ts.TotalDays);
                        break;
                    case "hour":
                        diff = Convert.ToDouble(ts.TotalHours);
                        break;
                    case "minute":
                        diff = Convert.ToDouble(ts.TotalMinutes);
                        break;
                    case "second":
                        diff = Convert.ToDouble(ts.TotalSeconds);
                        break;
                }
            }
            catch (Exception)
            {
                diff = 0;
            }
            return diff;
        }
        #endregion

    }
}
