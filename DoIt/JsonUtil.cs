using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using System.Reflection;

namespace DoIt
{
    public class JsonUtil
    {

        #region DataTable转成JSON
        /// <summary>
        /// DataTable转成JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtName"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt, string dtName)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                JsonSerializer ser = new JsonSerializer();
                jw.WriteStartObject();
                jw.WritePropertyName(dtName);
                jw.WriteStartArray();
                foreach (DataRow dr in dt.Rows)
                {
                    jw.WriteStartObject();

                    foreach (DataColumn dc in dt.Columns)
                    {
                        jw.WritePropertyName(dc.ColumnName);
                        ser.Serialize(jw, dr[dc].ToString());
                    }
                    jw.WriteEndObject();
                }
                jw.WriteEndArray();
                jw.WriteEndObject();

                sw.Close();
                jw.Close();
            }
            return sb.ToString();
        }
        #endregion

        #region ToStr拼接串

        /// <summary>
        /// 对象的全部属性和属性值。用于填写类json的{}内数据
        /// 生成后的格式类   属性1:'属性值'
        /// 将这些属性名和属性值写入字符串列表返回
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private List<string> GetObjectProperty(object o)
        {
            List<string> propertyslist = new List<string>();
            PropertyInfo[] propertys = o.GetType().GetProperties();
            foreach (PropertyInfo p in propertys)
            {
                if (p.GetValue(o, null) != null && !p.GetValue(o, null).ToString().Equals("0") && !p.GetValue(o, null).ToString().Equals("False"))
                {
                    if (p.PropertyType.Name.Equals("String"))
                    {
                        propertyslist.Add(p.Name.ToString() + ":'" + p.GetValue(o, null)+"'");
                    }
                    else{
                        propertyslist.Add(p.Name.ToString() + ":" + p.GetValue(o, null));
                    }
                }
            }
            return propertyslist;
        }
        /// <summary>
        /// 将一个对象的所有属性和属性值按类json的格式要求输入为一个封装后的结果。
        /// 返回值类似{属性1:'属性1值',属性2:属性2值,属性3:'属性3值'}
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string OneObjectToJson(object o)
        {
            string result = "{";
            List<string> lsPropertys = new List<string>();
            lsPropertys = GetObjectProperty(o);
            foreach (string strProperty in lsPropertys)
            {
                if (result.Equals("{"))
                {
                    result = result + strProperty;
                }
                else
                {
                    result = result + "," + strProperty + "";
                }
            }
            return result + "}";
        }

        #endregion

    }
}
