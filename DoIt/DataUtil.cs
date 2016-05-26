using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DoIt
{
    public class DataUtil<T> where T : class, new()
    {
        private string _modelName;
        public DataUtil(){
            _modelName = typeof(T).Name;
        }
        /// <summary>
        /// 获取实体类字段
        /// </summary>
        /// <param name="tObj"></param>
        /// <returns></returns>
        public string GetField(T tObj)
        {
            var properites = typeof(T).GetProperties();//得到实体类属性的集合
            return properites.Aggregate(string.Empty, (current, propertyInfo) => current + (propertyInfo.Name + ","));
        }
        /// <summary>
        /// 获取实体类值
        /// </summary>
        /// <param name="tObj"></param>
        /// <returns></returns>
        public string GetValue(T tObj)
        {
            var properties = typeof(T).GetProperties();
            return properties.Where(propertyInfo => !propertyInfo.GetValue(tObj, null).Equals(null)).Aggregate(string.Empty, (current, propertyInfo) => current + (propertyInfo.GetValue(tObj, null) + ","));
        }
        /// <summary>
        /// Reader转Objec
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="t"></param>
        public void ReaderToObject(IDataReader dr, T t)
        {
            for (var i = 0; i < dr.FieldCount; i++)
            {
                var propertyInfo = t.GetType().GetProperty(dr.GetName(i));
                if (propertyInfo == null) continue;
                if (dr.GetValue(i) != DBNull.Value)
                {
                    propertyInfo.SetValue(t, dr.GetValue(i), null);
                }
            }
        }
        /// <summary>
        /// Reader转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public List<T> ToList(IDataReader dr)
        {
            var type = typeof(T);
            var propertyinfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var list = new List<T>();
            while (dr.Read())
            {
                var t = new T();
                foreach (var property in propertyinfos)
                {
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        if (dr.GetName(i) != property.Name || dr.GetValue(i) == DBNull.Value) continue;
                        property.SetValue(t, dr.GetValue(i), null);
                        break;
                    }
                }
                list.Add(t);
            }
            return list;
        }
        /// <summary>
        /// List转换成DataSet
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">将要转换的List</param>
        /// <returns></returns>
        public DataSet ConvertToDataSet(IList<T> list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }

            var ds = new DataSet();
            var dt = new DataTable(typeof(T).Name);

            var myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var t in list.Where(t => t != null))
            {
                var row = dt.NewRow();

                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    var pi = myPropertyInfo[i];

                    var name = pi.Name;

                    if (dt.Columns[name] == null)
                    {
                        var column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }

                    row[name] = pi.GetValue(t, null);
                }

                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);

            return ds;

        } 
    }
}
