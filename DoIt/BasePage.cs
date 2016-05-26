using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoIt
{
    /// <summary>
    /// 基类
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (CookieUtil.GetCookieObj("lcp") == null)
            {
                FuncUtil.R("../Login/Index.aspx?url=" + Request.Url.PathAndQuery);
            }
            base.OnLoad(e);
        }
    }
}
