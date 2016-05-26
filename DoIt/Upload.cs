using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web;
using System.Drawing;

namespace DoIt
{

    #region 调用方法
    /*=======================================================================================
     * 使用方法：
     *      Upload u = new Upload();
     *      string tempstr = u.Open();
     =======================================================================================*/
    #endregion

    #region 文件上传类
    /// <summary> 
    /// 单文件上传类（图片）。 功能：上传文件操作(主要用于图片上传);
    /// </summary> 
    public class Upload
    {

        #region 初始参数
        private int _error = 0;//返回上传状态。 
        private int _maxSize = 5120000;//最大单个上传文件 (默认)
        private string _fileType = "jpg/gif/bmp/png";//所支持的上传类型用"/"隔开 
        private string _savePath = System.Web.HttpContext.Current.Server.MapPath("./upload/") + DateTime.Now.ToString("yyyy-MM-dd") + "/";//保存文件的实际路径 
        private int _saveType = 0;//上传文件的类型，0代表自动生成文件名 
        private HtmlInputFile _formFile;//上传控件。 
        private string _inFileName = "";//非自动生成文件名设置。 
        private string _outFileName = "";//输出文件名。 
        private bool _isCreateImg = true;//是否生成缩略图。 
        private bool _iss = false;//是否有缩略图生成.
        private int _height = 0;//获取上传图片的高度 
        private int _width = 0;//获取上传图片的宽度 
        private int _sHeight = 120;//设置生成缩略图的高度 
        private int _sWidth = 120;//设置生成缩略图的宽度
        private bool _isDraw = false;//设置是否加水印
        private int _drawStyle = 0;//设置加水印的方式０：文字水印模式，１：图片水印模式,2:不加
        private int _drawStringX = 10;//绘制文本的Ｘ坐标（左上角）
        private int _drawStringY = 10;//绘制文本的Ｙ坐标（左上角）
        private string _addText = "标注内容文字";//设置水印内容
        private string _font = "宋体";//设置水印字体
        private int _fontSize = 12;//设置水印字大小
        private int _fileSize = 0;//获取已经上传文件的大小
        private string _copyIamgePath = System.Web.HttpContext.Current.Server.MapPath(".") + "/images/watermark.jpg";//图片水印模式下的覆盖图片的实际地址

        /// <summary>
        /// Error返回值，1、没有上传的文件。2、类型不允许。3、大小超限。4、未知错误。0、上传成功。 
        /// </summary>
        public int Error
        {
            get { return _error; }
        }
        /// <summary>
        /// 最大单个上传文件
        /// </summary>
        public int MaxSize
        {
            set { _maxSize = value; }
        }
        /// <summary>
        /// 所支持的上传类型用"/"隔开 
        /// </summary>
        public string FileType
        {
            set { _fileType = value; }
        }
        /// <summary>
        /// //保存文件的实际路径 
        /// </summary>
        public string SavePath
        {
            set { _savePath = System.Web.HttpContext.Current.Server.MapPath(value); }
            get { return _savePath; }
        }
        /// <summary>
        /// 上传文件的类型，0代表自动生成文件名
        /// </summary>
        public int SaveType
        {
            set { _saveType = value; }
        }
        /// <summary>
        /// 上传控件
        /// </summary>
        public HtmlInputFile FormFile
        {
            set { _formFile = value; }
        }
        /// <summary>
        /// //非自动生成文件名设置。
        /// </summary>
        public string InFileName
        {
            set { _inFileName = value; }
        }
        /// <summary>
        /// 输出文件名
        /// </summary>
        public string OutFileName
        {
            get { return _outFileName; }
            set { _outFileName = value; }
        }
        /// <summary>
        /// 是否有缩略图生成.
        /// </summary>
        public bool Iss
        {
            get { return _iss; }
        }
        /// <summary>
        /// //获取上传图片的宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
        }
        /// <summary>
        /// //获取上传图片的高度
        /// </summary>
        public int Height
        {
            get { return _height; }
        }
        /// <summary>
        /// 设置缩略图的宽度
        /// </summary>
        public int SWidth
        {
            get { return _sWidth; }
            set { _sWidth = value; }
        }
        /// <summary>
        /// 设置缩略图的高度
        /// </summary>
        public int SHeight
        {
            get { return _sHeight; }
            set { _sHeight = value; }
        }
        /// <summary>
        /// 是否生成缩略图
        /// </summary>
        public bool IsCreateImg
        {
            get { return _isCreateImg; }
            set { _isCreateImg = value; }
        }
        /// <summary>
        /// 是否加水印
        /// </summary>
        public bool IsDraw
        {
            get { return _isDraw; }
            set { _isDraw = value; }
        }
        /// <summary>
        /// 设置加水印的方式０：文字水印模式，１：图片水印模式,2:不加
        /// </summary>
        public int DrawStyle
        {
            get { return _drawStyle; }
            set { _drawStyle = value; }
        }
        /// <summary>
        /// 绘制文本的Ｘ坐标（左上角）
        /// </summary>
        public int DrawStringX
        {
            get { return _drawStringX; }
            set { _drawStringX = value; }
        }
        /// <summary>
        /// 绘制文本的Ｙ坐标（左上角）
        /// </summary>
        public int DrawStringY
        {
            get { return _drawStringY; }
            set { _drawStringY = value; }
        }
        /// <summary>
        /// 设置文字水印内容
        /// </summary>
        public string AddText
        {
            get { return _addText; }
            set { _addText = value; }
        }
        /// <summary>
        /// 设置文字水印字体
        /// </summary>
        public string Font
        {
            get { return _font; }
            set { _font = value; }
        }
        /// <summary>
        /// 设置文字水印字的大小
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }
        /// <summary>
        /// 设置文件大小
        /// </summary>
        public int FileSize
        {
            get { return _fileSize; }
            set { _fileSize = value; }
        }
        /// <summary>
        /// 图片水印模式下的覆盖图片的实际地址
        /// </summary>
        public string CopyIamgePath
        {
            set { _copyIamgePath = System.Web.HttpContext.Current.Server.MapPath(value); }
        }

        #endregion

        #region 上传操作属性
        //获取文件的后缀名 
        private string GetExt(string path)
        {
            return Path.GetExtension(path);
        }
        //获取输出文件的文件名。 
        private string FileName(string ext)
        {
            if (_saveType == 0 || _inFileName.Trim() == "")
                return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(10000, 99999) + ext;
            else
                return _inFileName;
        }
        //检查上传的文件的类型，是否允许上传。 
        private bool IsUpload(string ext)
        {
            ext = ext.Replace(".", "");
            bool b = false;
            string[] arrFileType = _fileType.Split(';');
            foreach (string str in arrFileType)
            {
                if (str.ToLower() == ext.ToLower())
                {
                    b = true;
                    break;
                }
            }
            return b;
        }
        #endregion

        #region 上传基本操作
        /// <summary>
        /// 上传操作
        /// </summary>
        public string Open()
        {
            HttpPostedFile hpFile = _formFile.PostedFile;
            if (hpFile == null || hpFile.FileName.Trim() == "")
            {
                _error = 1;
                return "上传文件名不能为空！";
            }

            string ext = GetExt(hpFile.FileName);
            if (!IsUpload(ext))
            {
                _error = 2;
                return "上传文件扩展名不合法！";
            }

            int iLen = hpFile.ContentLength;
            if (iLen > _maxSize)
            {
                _error = 3;
                return "上传文件已经超出默认值！";
            }

            try
            {
                if (!Directory.Exists(_savePath))
                    Directory.CreateDirectory(_savePath);
                byte[] bData = new byte[iLen];
                hpFile.InputStream.Read(bData, 0, iLen);
                string fName;
                fName = FileName(ext);
                string tempFile = "";
                if (_isDraw)
                {
                    tempFile = fName.Split('.').GetValue(0).ToString() + "_temp." + fName.Split('.').GetValue(1).ToString();
                }
                else
                {
                    tempFile = fName;
                }
                FileStream newFile = new FileStream(_savePath + tempFile, FileMode.Create);
                newFile.Write(bData, 0, bData.Length);
                newFile.Flush();
                int fileSizeTemp = hpFile.ContentLength;
                //是否加水印
                if (_isDraw)
                {
                    //水印0为文字，1为图片，2为不加
                    if (_drawStyle == 0)
                    {
                        System.Drawing.Image img1 = System.Drawing.Image.FromStream(newFile);
                        Graphics g = Graphics.FromImage(img1);
                        g.DrawImage(img1, 100, 100, img1.Width, img1.Height);
                        Font f = new Font(_font, _fontSize);
                        Brush b = new SolidBrush(Color.Red);
                        string addtext = _addText;
                        g.DrawString(addtext, f, b, _drawStringX, _drawStringY);
                        g.Dispose();
                        img1.Save(_savePath + fName);
                        img1.Dispose();
                    }
                    else if (_drawStyle == 1)
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromStream(newFile);
                        System.Drawing.Image copyImage = System.Drawing.Image.FromFile(_copyIamgePath);
                        Graphics g = Graphics.FromImage(image);
                        g.DrawImage(copyImage, new Rectangle(image.Width - copyImage.Width - 5, image.Height - copyImage.Height - 5, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                        g.Dispose();
                        image.Save(_savePath + fName);
                        image.Dispose();
                    }
                }

                try
                {
                    //获取图片的高度和宽度
                    System.Drawing.Image img = System.Drawing.Image.FromStream(newFile);
                    _width = img.Width;
                    _height = img.Height;
                    //生成缩略图部分 
                    if (_isCreateImg)
                    {
                        //如果上传文件小于15k，则不生成缩略图。 
                        if (iLen > 15360)
                        {
                            System.Drawing.Image newImg = img.GetThumbnailImage(_sWidth, _sHeight, null, System.IntPtr.Zero);
                            newImg.Save(_savePath + fName.Split('.').GetValue(0).ToString() + "_s." + fName.Split('.').GetValue(1).ToString());
                            newImg.Dispose();
                            _iss = true;
                        }
                    }
                    if (_isDraw)
                    {
                        if (File.Exists(_savePath + fName.Split('.').GetValue(0).ToString() + "_temp." + fName.Split('.').GetValue(1).ToString()))
                        {
                            newFile.Dispose();
                            File.Delete(_savePath + fName.Split('.').GetValue(0).ToString() + "_temp." + fName.Split('.').GetValue(1).ToString());
                        }
                    }
                }
                catch { }
                newFile.Close();
                newFile.Dispose();
                _outFileName = fName;
                _fileSize = fileSizeTemp;
                _error = 0;
                return "文件上传成功！";
            }
            catch
            {
                _error = 4;
                return "文件上传失败！";
            }
        }
        #endregion

    }
    #endregion

}
