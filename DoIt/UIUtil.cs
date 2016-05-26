using System.Web.UI.WebControls;
using System.Collections;

namespace DoIt
{
    public class UiUtil
    {
        /// <summary>
        /// 封装下拉列表，列表框，多选列表，单选列表和项目符号列表
        /// DropDownList、ListBox、CheckBoxList、RadioButtonList 和 BulletedList
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="obj"></param>
        /// <param name="valueField"></param>
        /// <param name="textField"></param>
        public static void BindList(ListControl ctrl, object obj, string valueField, string textField)
        {
            ctrl.DataSource = obj;
            ctrl.DataValueField = valueField;
            ctrl.DataTextField = textField;
            ctrl.DataBind();
        }
        /// <summary>
        /// 封装下拉列表
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="valueField"></param>
        /// <param name="textField"></param>
        public static void BindList(ListControl ctrl, string valueField, string textField)
        {
            ctrl.Items.Add(new ListItem(textField, valueField));
        }
        /// <summary>
        /// 初始化绑定权限列表，字符串
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="strapp"></param>
        /// <param name="flag"></param>
        public static void BindString(ListControl ddl, string strapp, string flag)
        {
            string[] tempstr = strapp.Replace(flag, "|").Split('|');
            for (int i = 0; i < tempstr.Length; i++)
            {
                ddl.Items.Add(new ListItem(tempstr[i].ToString(), (i + 1).ToString()));
            }
        }
        /// <summary>
        /// 初始化绑定权限列表，集合表
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="list"></param>
        public static void BindString(ListControl ddl, IList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ddl.Items.Add(new ListItem(list[i].ToString(), (i + 1).ToString()));
            }
        }
        /// <summary>
        /// 初始化绑定权限列表，Hashtable
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="ht"></param>
        public static void BindString(ListControl ddl, Hashtable ht)
        {
            ArrayList list = new ArrayList(ht.Keys);
            list.Sort();
            foreach (string str in list)
            {
                ddl.Items.Add(new ListItem(ht[str].ToString(), str));
            }
        }
        /// <summary>
        /// 获取勾选列表
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string ListString(ListControl ddl, string flag)
        {
            string tempstr = string.Empty;
            foreach (ListItem li in ddl.Items)
            {
                if (li.Selected == true)
                {
                    tempstr += string.Format("{0}{1}", li.Value, flag);
                }
            }
            return tempstr;
        }
        /// <summary>
        /// 获取多选框值列表
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="strapp"></param>
        /// <param name="flag"></param>
        public static void ListCheck(ListControl ddl, string strapp, string flag)
        {
            if (strapp != null)
            {
                string[] tempstr = strapp.Replace(flag, "|").Split('|');
                foreach (string str in tempstr)
                {
                    for (int i = 0; i < ddl.Items.Count; i++)
                    {
                        if (ddl.Items[i].Value == str)
                        {
                            ddl.Items[i].Selected = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取页面权限
        /// </summary>
        /// <param name="rights"></param>
        /// <param name="tempstr"></param>
        /// <returns></returns>
        public static bool PageRight(string rights, string tempstr)
        {
            bool flag = false;
            if (rights != null && rights.IndexOf(tempstr) > -1)
            {
                flag = true;
            }
            return flag;
        }
    }
}
