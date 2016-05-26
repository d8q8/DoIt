using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace DoIt
{
    /// <summary>
    /// 提供向页面输出客户端代码实现特殊功能的方法
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class JScriptUtil
    {

        #region JS处理函数
        /// <summary>
        /// 普通信息弹窗提示
        /// </summary>
        /// <param name="page"></param>
        /// <param name="model"></param>
        public static void Alert(System.Web.UI.Page page, Pop model)
        {
            Show(page, "alert", model);
        }

        /// <summary>
        /// 成功信息弹窗提示
        /// </summary>
        /// <param name="page"></param>
        /// <param name="model"></param>
        public static void Success(System.Web.UI.Page page, Pop model)
        {
            Show(page, "succeedInfo", model);
        }
        /// <summary>
        /// 失败信息弹窗提示
        /// </summary>
        /// <param name="page"></param>
        /// <param name="model"></param>
        public static void Error(System.Web.UI.Page page, Pop model)
        {
            Show(page, "errorInfo", model);
        }
        /// <summary>
        /// 询问信息弹窗提示
        /// </summary>
        /// <param name="page"></param>
        /// <param name="model"></param>
        public static void Confirm(System.Web.UI.Page page, Pop model)
        {
            Show(page, "confirmInfo", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="model"></param>
        public static void Win(System.Web.UI.Page page, Pop model)
        {
            Show(page, "win", model);
        }

        /// <summary>
        /// 显示消息提示对话框
        /// </summary>
        private static void Show(System.Web.UI.Page page, string messagetype, Pop model)
        {
            JsonUtil json = new JsonUtil();
            string script = string.Format("ymPrompt.{0}({1});", messagetype, json.OneObjectToJson(model));
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", script, true);
        }

        #endregion

    }
    /// <summary>
    /// 弹窗实体类
    /// </summary>
    public class Pop
    {

        #region 弹窗基本参数
        //消息框按钮
        public string message { get; set; }
        //宽
        public int width { get; set; }
        //高
        public int height { get; set; }
        //消息框标题
        public string title { get; set; }
        //回调事件
        public object handler { get; set; }
        //遮罩透明色
        public string maskAlphaColor { get; set; }
        //遮罩透明度
        public decimal maskAlpha { get; set; }
        //iframe模式
        public bool iframe { get; set; }
        //图标的样式
        public string icoCls { get; set; }
        //按钮配置
        public object btn { get; set; }
        //随滚动条滚动
        public bool fixPosition { get; set; }
        //点击关闭、确定等按钮后自动关闭 
        public bool autoClose { get; set; }
        //不允许拖出窗体范围 
        public bool dragOut { get; set; }
        //显示标题栏
        public bool titleBar { get; set; }
        //显示遮罩 
        public bool showMask { get; set; }
        //在页面中间显示 
        public string winPos { get; set; }
        //拖动窗体时窗体的透明度
        public decimal winAlpha { get; set; }
        //是否显示关闭按钮
        public bool closeBtn { get; set; }
        //不显示阴影，只对IE有效
        public bool showShadow { get; set; }
        //不使用淡入淡出 
        public bool useSlide { get; set; }
        //淡入淡出配置 
        public object slideCfg { get; set; }
        //按钮文本，可通过自定义这些属性实现本地化
        public string closeTxt { get; set; }
        //确定按钮
        public string okTxt { get; set; }
        //取消按钮
        public string cancelTxt { get; set; }
        //消息内容的样式 
        public string msgCls { get; set; }
        //不显示最小化按钮
        public bool minBtn { get; set; }
        //最少化
        public string minTxt { get; set; }
        //不显示最大化按钮 
        public bool maxBtn { get; set; }
        //最大化
        public string maxTxt { get; set; }
        //是否允许选择消息框内容，默认不允许 
        public bool allowSelect { get; set; }
        //是否允许在消息框使用右键，默认不允许
        public bool allowRightMenu { get; set; }
        #endregion

    }
}
