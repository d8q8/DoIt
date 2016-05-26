using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DoIt
{
    public class FilesUtil : IDisposable
    {
        private bool _alreadyDispose;

        #region 构造函数

        ~FilesUtil()
        {
            Dispose();
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose) return;
            _alreadyDispose = true;
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 取得文件后缀名
        /****************************************
         * 函数名称：GetPostfixStr
         * 功能说明：取得文件后缀名
         * 参    数：filename:文件名称
         * 调用示列：
         *           string filename = "aaa.aspx";        
         *           string s = DoIt.FilesUtil.GetPostfixStr(filename);         
        *****************************************/
        /// <summary>
        /// 取后缀名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>.gif|.html格式</returns>
        public static string GetPostfixStr(string filename)
        {
            var start = filename.LastIndexOf(".", StringComparison.Ordinal);
            var length = filename.Length;
            var postfix = filename.Substring(start, length - start);
            return postfix;
        }
        #endregion

        #region 写文件
        /****************************************
         * 函数名称：WriteFile
         * 功能说明：写文件,会覆盖掉以前的内容
         * 参    数：Path:文件路径,Strings:文本内容
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string Strings = "这是我写的内容啊";
         *           DoIt.FilesUtil.WriteFile(Path,Strings);
        *****************************************/
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="strings">文件内容</param>
        public static void WriteFile(string path, string strings)
        {
            if (!File.Exists(path))
            {
                var f = File.Create(path);
                f.Close();
            }
            var f2 = new StreamWriter(path, false, Encoding.GetEncoding("gb2312"));
            f2.Write(strings);
            f2.Close();
            f2.Dispose();
        }
        #endregion

        #region 读文件
        /****************************************
         * 函数名称：ReadFile
         * 功能说明：读取文本内容
         * 参    数：Path:文件路径
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string s = DoIt.FilesUtil.ReadFile(Path);
        *****************************************/
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            string s;
            if (!File.Exists(path))
                s = "不存在相应的目录";
            else
            {
                var f2 = new StreamReader(path, Encoding.GetEncoding("gb2312"));
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        #endregion

        #region 追加文件
        /****************************************
         * 函数名称：FileAdd
         * 功能说明：追加文件内容
         * 参    数：Path:文件路径,strings:内容
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");     
         *           string Strings = "新追加内容";
         *           DoIt.FilesUtil.FileAdd(Path, Strings);
        *****************************************/
        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="strings">内容</param>
        public static void FileAdd(string path, string strings)
        {
            var sw = File.AppendText(path);
            sw.Write(strings);
            sw.Flush();
            sw.Close();
        }
        #endregion

        #region 拷贝文件
        /****************************************
         * 函数名称：FileCoppy
         * 功能说明：拷贝文件
         * 参    数：OrignFile:原始文件,NewFile:新文件路径
         * 调用示列：
         *           string orignFile = Server.MapPath("Default2.aspx");     
         *           string NewFile = Server.MapPath("Default3.aspx");
         *           DoIt.FilesUtil.FileCoppy(OrignFile, NewFile);
        *****************************************/
        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="orignFile">原始文件</param>
        /// <param name="newFile">新文件路径</param>
        public static void FileCopy(string orignFile, string newFile)
        {
            File.Copy(orignFile, newFile, true);
        }

        #endregion

        #region 删除文件
        /****************************************
         * 函数名称：FileDel
         * 功能说明：删除文件
         * 参    数：Path:文件路径
         * 调用示列：
         *           string Path = Server.MapPath("Default3.aspx");    
         *           DoIt.FilesUtil.FileDel(Path);
        *****************************************/
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        public static void FileDel(string path)
        {
            File.Delete(path);
        }
        #endregion

        #region 移动文件
        /****************************************
         * 函数名称：FileMove
         * 功能说明：移动文件
         * 参    数：OrignFile:原始路径,NewFile:新文件路径
         * 调用示列：
         *            string orignFile = Server.MapPath("../说明.txt");    
         *            string NewFile = Server.MapPath("http://www.d8q8.com/说明.txt");
         *            DoIt.FilesUtil.FileMove(OrignFile, NewFile);
        *****************************************/
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="orignFile">原始路径</param>
        /// <param name="newFile">新路径</param>
        public static void FileMove(string orignFile, string newFile)
        {
            File.Move(orignFile, newFile);
        }
        #endregion

        #region 在当前目录下创建目录
        /****************************************
         * 函数名称：FolderCreate
         * 功能说明：在当前目录下创建目录
         * 参    数：OrignFolder:当前目录,NewFloder:新目录
         * 调用示列：
         *           string orignFolder = Server.MapPath("test/");    
         *           string NewFloder = "new";
         *           DoIt.FilesUtil.FolderCreate(OrignFolder, NewFloder); 
        *****************************************/
        /// <summary>
        /// 在当前目录下创建目录
        /// </summary>
        /// <param name="orignFolder">当前目录</param>
        /// <param name="newFloder">新目录</param>
        public static void FolderCreate(string orignFolder, string newFloder)
        {
            Directory.SetCurrentDirectory(orignFolder);
            Directory.CreateDirectory(newFloder);
        }
        #endregion

        #region 递归删除文件夹目录及文件
        /****************************************
         * 函数名称：DeleteFolder
         * 功能说明：递归删除文件夹目录及文件
         * 参    数：dir:文件夹路径
         * 调用示列：
         *           string dir = Server.MapPath("test/");  
         *           DoIt.FilesUtil.DeleteFolder(dir);       
        *****************************************/
        /// <summary>
        /// 递归删除文件夹目录及文件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static void DeleteFolder(string dir)
        {
            if (!Directory.Exists(dir)) return;
            foreach (var d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                    File.Delete(d); //直接删除其中的文件 
                else
                    DeleteFolder(d); //递归删除子文件夹 
            }
            Directory.Delete(dir); //删除已空文件夹 
        }

        #endregion

        #region 将指定文件夹下面的所有内容copy到目标文件夹下面,如果目标文件夹为只读属性就会报错。
        /****************************************
         * 函数名称：CopyDir
         * 功能说明：将指定文件夹下面的所有内容copy到目标文件夹下面如果目标文件夹为只读属性就会报错。
         * 参    数：srcPath:原始路径,aimPath:目标文件夹
         * 调用示列：
         *           string srcPath = Server.MapPath("test/");  
         *           string aimPath = Server.MapPath("test1/");
         *           DoIt.FilesUtil.CopyDir(srcPath,aimPath);   
        *****************************************/
        /// <summary>
        /// 指定文件夹下面的所有内容copy到目标文件夹下面
        /// </summary>
        /// <param name="srcPath">原始路径</param>
        /// <param name="aimPath">目标文件夹</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                //如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                var fileList = Directory.GetFileSystemEntries(srcPath);
                //遍历所有的文件和目录
                foreach (var file in fileList)
                {
                    //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    //否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }
        #endregion

        #region 获取指定文件夹下所有子目录及文件(树形)
        /****************************************
         * 函数名称：GetFoldAll(string Path)
         * 功能说明：获取指定文件夹下所有子目录及文件(树形)
         * 参    数：Path:详细路径
         * 调用示列：
         *           string strDirlist = Server.MapPath("templates");      
         *           this.Literal1.Text = DoIt.FilesUtil.GetFoldAll(strDirlist);
        *****************************************/
        /// <summary>
        /// 获取指定文件夹下所有子目录及文件
        /// </summary>
        /// <param name="path">详细路径</param>
        public static string GetFoldAll(string path)
        {
            var str = "";
            var thisOne = new DirectoryInfo(path);
            str = ListTreeShow(thisOne, 0, str);
            return str;
        }

        /// <summary>
        /// 获取指定文件夹下所有子目录及文件函数
        /// </summary>
        /// <param name="theDir">指定目录</param>
        /// <param name="nLevel">默认起始值,调用时,一般为0</param>
        /// <param name="rn">用于迭加的传入值,一般为空</param>
        /// <returns></returns>
        private static string ListTreeShow(DirectoryInfo theDir, int nLevel, string rn)//递归目录 文件
        {
            var subDirectories = theDir.GetDirectories();//获得目录
            foreach (var dirinfo in subDirectories)
            {

                if (nLevel == 0)
                {
                    rn += "├";
                }
                else
                {
                    var s = "";
                    for (var i = 1; i <= nLevel; i++)
                    {
                        s += "│&nbsp;";
                    }
                    rn += s + "├";
                }
                rn += $"<strong>{dirinfo.Name}</strong><br />";
                var fileInfo = dirinfo.GetFiles();   //目录下的文件
                foreach (var fInfo in fileInfo)
                {
                    if (nLevel == 0)
                    {
                        rn += "│&nbsp;├";
                    }
                    else
                    {
                        var f = "";
                        for (var i = 1; i <= nLevel; i++)
                        {
                            f += "│&nbsp;";
                        }
                        rn += f + "│&nbsp;├";
                    }
                    rn += fInfo.Name + " <br />";
                }
                rn = ListTreeShow(dirinfo, nLevel + 1, rn);
            }
            return rn;
        }
        #endregion

        #region 获取指定文件夹下所有子目录及文件(下拉框形)
        /****************************************
         * 函数名称：GetFoldAll(string Path, string DropName, string tplPath)
         * 功能说明：获取指定文件夹下所有子目录及文件(下拉框形)
         * 参    数：Path:详细路径
         * 调用示列：
         *            string strDirlist = Server.MapPath("templates");     
         *            this.Literal2.Text = DoIt.FilesUtil.GetFoldAll(strDirlist,"tpl","");
        *****************************************/
        /// <summary>
        /// 获取指定文件夹下所有子目录及文件(下拉框形)
        /// </summary>
        /// <param name="path">详细路径</param>
        ///<param name="dropName">下拉列表名称</param>
        ///<param name="tplPath">默认选择模板名称</param>
        public static string GetFoldAll(string path, string dropName, string tplPath)
        {
            var strDrop = "<select name=\"" + dropName + "\" id=\"" + dropName + "\"><option value=\"\">--请选择详细模板--</option>";
            var str = "";
            var thisOne = new DirectoryInfo(path);
            str = ListTreeShow(thisOne, 0, str, tplPath);
            return strDrop + str + "</select>";

        }

        /// <summary>
        /// 获取指定文件夹下所有子目录及文件函数
        /// </summary>
        /// <param name="theDir">指定目录</param>
        /// <param name="nLevel">默认起始值,调用时,一般为0</param>
        /// <param name="rn">用于迭加的传入值,一般为空</param>
        /// <param name="tplPath">默认选择模板名称</param>
        /// <returns></returns>
        private static string ListTreeShow(DirectoryInfo theDir, int nLevel, string rn, string tplPath)//递归目录 文件
        {
            var subDirectories = theDir.GetDirectories();//获得目录

            foreach (var dirinfo in subDirectories)
            {

                rn += "<option value=\"" + dirinfo.Name + "\"";
                if (String.Equals(tplPath, dirinfo.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    rn += " selected ";
                }
                rn += ">";

                if (nLevel == 0)
                {
                    rn += "┣";
                }
                else
                {
                    var s = "";
                    for (var i = 1; i <= nLevel; i++)
                    {
                        s += "│&nbsp;";
                    }
                    rn += s + "┣";
                }
                rn += "" + dirinfo.Name + "</option>";


                var fileInfo = dirinfo.GetFiles();   //目录下的文件
                foreach (var fInfo in fileInfo)
                {
                    rn += "<option value=\"" + dirinfo.Name + "/" + fInfo.Name + "\"";
                    if (string.Equals(tplPath, fInfo.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        rn += " selected ";
                    }
                    rn += ">";

                    if (nLevel == 0)
                    {
                        rn += "│&nbsp;├";
                    }
                    else
                    {
                        var f = "";
                        for (var i = 1; i <= nLevel; i++)
                        {
                            f += "│&nbsp;";
                        }
                        rn += f + "│&nbsp;├";
                    }
                    rn += fInfo.Name + "</option>";
                }
                rn = ListTreeShow(dirinfo, nLevel + 1, rn, tplPath);
            }
            return rn;
        }
        #endregion

        #region 获取文件夹大小
        /****************************************
         * 函数名称：GetDirectoryLength(string dirPath)
         * 功能说明：获取文件夹大小
         * 参    数：dirPath:文件夹详细路径
         * 调用示列：
         *           string Path = Server.MapPath("templates");
         *           Response.Write(DoIt.FilesUtil.GetDirectoryLength(Path));      
        *****************************************/
        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                return 0;
            var di = new DirectoryInfo(dirPath);
            var len = di.GetFiles().Sum(fi => fi.Length);
            var dis = di.GetDirectories();
            if (dis.Length <= 0) return len;
            len += dis.Sum(t => GetDirectoryLength(t.FullName));
            return len;
        }
        #endregion

        #region 获取指定文件详细属性
        /****************************************
         * 函数名称：GetFileAttibe(string filePath)
         * 功能说明：获取指定文件详细属性
         * 参    数：filePath:文件详细路径
         * 调用示列：
         *           string file = Server.MapPath("robots.txt");
         *            Response.Write(DoIt.FilesUtil.GetFileAttibe(file));        
        *****************************************/
        /// <summary>
        /// 获取指定文件详细属性
        /// </summary>
        /// <param name="filePath">文件详细路径</param>
        /// <returns></returns>
        public static string GetFileAttibe(string filePath)
        {
            var str = "";
            var objFi = new FileInfo(filePath);
            str += "详细路径:" + objFi.FullName + "<br>文件名称:" + objFi.Name + "<br>文件长度:" + objFi.Length.ToString() + "字节<br>创建时间" + objFi.CreationTime.ToString(CultureInfo.InvariantCulture) + "<br>最后访问时间:" + objFi.LastAccessTime.ToString(CultureInfo.InvariantCulture) + "<br>修改时间:" + objFi.LastWriteTime.ToString(CultureInfo.InvariantCulture) + "<br>所在目录:" + objFi.DirectoryName + "<br>扩展名:" + objFi.Extension;
            return str;
        }
        #endregion

    }
}