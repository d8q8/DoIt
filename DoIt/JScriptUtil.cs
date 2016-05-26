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

        #region 基本参数
        private string _message;
        private int _width;
        private int _height;
        private string _title;
        private object _handler;
        private string _maskAlphaColor;
        private decimal _maskAlpha;
        private bool _iframe;
        private string _icoCls;
        private object _btn;
        private bool _fixPosition;
        private bool _autoClose;
        private bool _dragOut;
        private bool _titleBar;
        private bool _showMask;
        private string _winPos;
        private decimal _winAlpha;
        private bool _closeBtn;
        private bool _showShadow;
        private bool _useSlide;
        private object _slideCfg;
        private string _closeTxt;
        private string _okTxt;
        private string _cancelTxt;
        private string _msgCls;
        private bool _minBtn;
        private string _minTxt;
        private bool _maxBtn;
        private string _maxTxt;
        private bool _allowSelect;
        private bool _allowRightMenu;
        #endregion

        #region 基本属性
        /// <summary>
        /// 消息框按钮
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 宽 
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// 高
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// 消息框标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 回调事件 
        /// </summary>
        public object Handler
        {
            get { return _handler; }
            set { _handler = value; }
        }

        /// <summary>
        /// 遮罩透明色 
        /// </summary>
        public string MaskAlphaColor
        {
            get { return _maskAlphaColor; }
            set { _maskAlphaColor = value; }
        }

        /// <summary>
        /// 遮罩透明度 
        /// </summary>
        public decimal MaskAlpha
        {
            get { return _maskAlpha; }
            set { _maskAlpha = value; }
        }

        /// <summary>
        /// iframe模式 
        /// </summary>
        public bool Iframe
        {
            get { return _iframe; }
            set { _iframe = value; }
        }

        /// <summary>
        /// 图标的样式 
        /// </summary>
        public string IcoCls
        {
            get { return _icoCls; }
            set { _icoCls = value; }
        }

        /// <summary>
        /// 按钮配置 
        /// </summary>
        public object Btn
        {
            get { return _btn; }
            set { _btn = value; }
        }

        /// <summary>
        /// 点击关闭、确定等按钮后自动关闭 
        /// </summary>
        public bool AutoClose
        {
            get { return _autoClose; }
            set { _autoClose = value; }
        }

        /// <summary>
        /// 随滚动条滚动 
        /// </summary>
        public bool FixPosition
        {
            get { return _fixPosition; }
            set { _fixPosition = value; }
        }

        /// <summary>
        /// 不允许拖出窗体范围 
        /// </summary>
        public bool DragOut
        {
            get { return _dragOut; }
            set { _dragOut = value; }
        }

        /// <summary>
        /// 显示标题栏
        /// </summary>
        public bool TitleBar
        {
            get { return _titleBar; }
            set { _titleBar = value; }
        }

        /// <summary>
        /// 显示遮罩 
        /// </summary>
        public bool ShowMask
        {
            get { return _showMask; }
            set { _showMask = value; }
        }

        /// <summary>
        /// 在页面中间显示 
        /// </summary>
        public string WinPos
        {
            get { return _winPos; }
            set { _winPos = value; }
        }

        /// <summary>
        /// 拖动窗体时窗体的透明度 
        /// </summary>
        public decimal WinAlpha
        {
            get { return _winAlpha; }
            set { _winAlpha = value; }
        }

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool CloseBtn
        {
            get { return _closeBtn; }
            set { _closeBtn = value; }
        }

        /// <summary>
        /// 不显示阴影，只对IE有效 
        /// </summary>
        public bool ShowShadow
        {
            get { return _showShadow; }
            set { _showShadow = value; }
        }

        /// <summary>
        /// 不使用淡入淡出 
        /// </summary>
        public bool UseSlide
        {
            get { return _useSlide; }
            set { _useSlide = value; }
        }

        /// <summary>
        /// 淡入淡出配置 
        /// </summary>
        public object SlideCfg
        {
            get { return _slideCfg; }
            set { _slideCfg = value; }
        }

        //按钮文本，可通过自定义这些属性实现本地化 
        /// <summary>
        /// 关闭按钮
        /// </summary>
        public string CloseTxt
        {
            get { return _closeTxt; }
            set { _closeTxt = value; }
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        public string OkTxt
        {
            get { return _okTxt; }
            set { _okTxt = value; }
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        public string CancelTxt
        {
            get { return _cancelTxt; }
            set { _cancelTxt = value; }
        }

        /// <summary>
        /// 消息内容的样式 
        /// </summary>
        public string MsgCls
        {
            get { return _msgCls; }
            set { _msgCls = value; }
        }

        /// <summary>
        /// 不显示最小化按钮 
        /// </summary>
        public bool MinBtn
        {
            get { return _minBtn; }
            set { _minBtn = value; }
        }

        /// <summary>
        /// 最少化
        /// </summary>
        public string MinTxt
        {
            get { return _minTxt; }
            set { _minTxt = value; }
        }

        /// <summary>
        /// 不显示最大化按钮 
        /// </summary>
        public bool MaxBtn
        {
            get { return _maxBtn; }
            set { _maxBtn = value; }
        }

        /// <summary>
        /// 最大化
        /// </summary>
        public string MaxTxt
        {
            get { return _maxTxt; }
            set { _maxTxt = value; }
        }

        /// <summary>
        /// 是否允许选择消息框内容，默认不允许 
        /// </summary>
        public bool AllowSelect
        {
            get { return _allowSelect; }
            set { _allowSelect = value; }
        }

        /// <summary>
        /// 是否允许在消息框使用右键，默认不允许
        /// </summary>
        public bool AllowRightMenu
        {
            get { return _allowRightMenu; }
            set { _allowRightMenu = value; }
        }
        #endregion

    }
}
