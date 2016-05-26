using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace DoIt
{
    /// <summary> 
    /// Cookie操作类 
    /// </summary> 
    public class CookieUtil
    {

        #region COOKIES
        /// <summary> 
        /// 保存一个Cookie 
        /// </summary> 
        /// <param name="cookieName">Cookie名称</param> 
        /// <param name="cookieValue">Cookie值</param> 
        /// <param name="cookieTime">Cookie过期时间(小时),0为关闭页面失效</param> 
        public static void SaveCookie(string cookieName, string cookieValue, double cookieTime)
        {
            HttpCookie myCookie = new HttpCookie(cookieName);
            DateTime now = DateTime.Now;
            myCookie.Value = cookieValue;

            if (cookieTime != 0)
            {
                //有两种方法，第一方法设置Cookie时间的话，关闭浏览器不会自动清除Cookie 
                //第二方法不设置Cookie时间的话，关闭浏览器会自动清除Cookie ,但是有效期 
                //多久还未得到证实。 
                myCookie.Expires = now.AddDays(cookieTime);
                if (HttpContext.Current.Response.Cookies[cookieName] != null)
                    HttpContext.Current.Response.Cookies.Remove(cookieName);

                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            else
            {
                if (HttpContext.Current.Response.Cookies[cookieName] != null)
                    HttpContext.Current.Response.Cookies.Remove(cookieName);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
        /// <summary>
        /// 取得Cookies对象
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static HttpCookie GetCookieObj(string cookieName)
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (myCookie != null)
                return myCookie;
            else
                return null;
        }
        /// <summary> 
        /// 取得单个CookieValue 
        /// </summary> 
        /// <param name="cookieName">Cookie名称</param> 
        /// <returns>Cookie的值</returns> 
        public static string GetCookie(string cookieName)
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (myCookie != null)
                return myCookie.Value;
            else
                return null;
        }
        /// <summary>
        /// 取得多个CookiesValue
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName,string value)
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (myCookie != null)
                return myCookie.Values[value].ToString();
            else
                return null;
        }
        /// <summary> 
        /// 清除CookieValue 
        /// </summary> 
        /// <param name="cookieName">Cookie名称</param> 
        public static void ClearCookie(string cookieName)
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[cookieName];
            myCookie.Expires = DateTime.Now.AddYears(-2);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }
        #endregion

        #region 序列化与反序列化
        /// <summary>
        /// 序列化实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T obj)
        {
            System.Runtime.Serialization.IFormatter bf = new BinaryFormatter();
            string result = string.Empty;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bf.Serialize(ms, obj);
                byte[] byt = new byte[ms.Length];
                byt = ms.ToArray();
                result = System.Convert.ToBase64String(byt);
                ms.Flush();
            }
            return result;
        }
        /// <summary>
        /// 反序列化实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string str)
        {
            T obj;
            System.Runtime.Serialization.IFormatter bf = new BinaryFormatter();
            byte[] byt = Convert.FromBase64String(str);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byt, 0, byt.Length))
            {
                obj = (T)bf.Deserialize(ms);
            }
            return obj;
        }
        /// <summary>
        /// 序列化任何对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(Object obj)
        {
            BinaryFormatter ser = new BinaryFormatter();
            MemoryStream mStream = new MemoryStream();
            ser.Serialize(mStream, obj);
            byte[] buf = mStream.ToArray();
            mStream.Close();
            return Convert.ToBase64String(buf);
        }
        /// <summary>
        /// 反序列化任何对象
        /// </summary>
        /// <param name="strLoginUserModel"></param>
        /// <returns></returns>
        public static object DeserializeObject(string strLoginUserModel)
        {
            byte[] binary = Convert.FromBase64String(strLoginUserModel);
            BinaryFormatter ser = new BinaryFormatter();
            MemoryStream mStream = new MemoryStream(binary);
            object obj = new object();
            obj = (object)ser.Deserialize(mStream);
            mStream.Close();
            return obj;
        }
        #endregion

    }
}
