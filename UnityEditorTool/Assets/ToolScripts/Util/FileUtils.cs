
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace BearG {

    public class FileUtils {
        public static string ReadText(string path) {
            if (File.Exists(path)) {
                var content = File.ReadAllText(path);
                return content;
            }
            else {
                return null;
            }
        }
        public static void CreatText(string path, string content) {
            File.WriteAllText(path, content);
        }
        public static void WriteText(string path, string content) {
            if (!File.Exists(path)) {
                CreatText(path, content);
            }
            else {
                string[] contents = new string[]
                            { content };
                File.AppendAllLines(path, contents);
            }
        }
        public static void CopyDirectory(string sourceDirectory, string destDirectory) {
            if (!Directory.Exists(sourceDirectory)) {
                return;
            }
            if (!Directory.Exists(destDirectory)) {
                Directory.CreateDirectory(destDirectory);
            }
            //拷贝文件
            CopyFile(sourceDirectory, destDirectory);
            //递归拷贝子目录       
            string[] directionName = Directory.GetDirectories(sourceDirectory);
            foreach (string directionPath in directionName) {
                string directionPathTemp = Path.Combine(destDirectory, directionPath.Substring(sourceDirectory.Length + 1));
                CopyDirectory(directionPath, directionPathTemp);
            }
        }
        public static void CopyFile(string sourceDirectory, string destDirectory) {
            string[] fileName = Directory.GetFiles(sourceDirectory);
            foreach (string filePath in fileName) {
                //根据每个文件名称生成对应的目标文件名称
                string filePathTemp = Path.Combine(destDirectory, filePath.Substring(sourceDirectory.Length + 1));
                //存在则覆盖复制
                if (File.Exists(filePathTemp)) {
                    File.Copy(filePath, filePathTemp, true);
                }
                else {
                    File.Copy(filePath, filePathTemp);
                }
            }
        }

        #region OpenFolder
        public static void OpenDirectory(string path) {
            if (string.IsNullOrEmpty(path)) return;
            if (!Directory.Exists(path)) {
                UnityEngine.Debug.LogError("No Directory: " + path);
                return;
            }
            //Application.dataPath 只能在主线程中获取
            int lastIndex = Application.dataPath.LastIndexOf("/");
            // 新开线程防止锁死
            Thread newThread = new Thread(new ParameterizedThreadStart(CmdOpenDirectory));
            newThread.Start(path);
        }

        private static void CmdOpenDirectory(object obj) {
            Process p = new Process();
#if UNITY_EDITOR_WIN
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c start " + obj.ToString();
#elif UNITY_EDITOR_OSX
        string shellPath = Application.dataPath.Substring(0, lastIndex) + "/Shell/";
        p.StartInfo.FileName = "bash";
        string shPath = shellPath + "openDir.sh";
        p.StartInfo.Arguments = shPath + " " + obj.ToString();
#endif
            //UnityEngine.Debug.Log(p.StartInfo.Arguments);
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            p.WaitForExit();
            p.Close();
        }
    #endregion

        #region OpenFilePath
        public class PathBrowser {
            // 浏览对话框中包含一个编辑框，在该编辑框中用户可以输入选中项的名字。
            const int BIF_EDITBOX = 0x00000010;
            // 新用户界面
            const int BIF_NEWDIALOGSTYLE = 0x00000040;
            const int BIF_USENEWUI = (BIF_NEWDIALOGSTYLE | BIF_EDITBOX);
            const int MAX_PATH_LENGTH = 2048;
            public static string FolderBrowserDlg(string defaultPath = "") {
                OpenDlgDir dlg = new OpenDlgDir();
                dlg.pszDisplayName = defaultPath;
                dlg.ulFlags = BIF_USENEWUI;
                //设置hwndOwner==0时，是非模态对话框，设置hwndOwner!=0时为模态对话框
                dlg.hwndOwner = DllOpenFileDialog.GetForegroundWindow();

                IntPtr pidlPtr = DllOpenFileDialog.SHBrowseForFolder(dlg);
                char[] charArray = new char[MAX_PATH_LENGTH];
                DllOpenFileDialog.SHGetPathFromIDList(pidlPtr, charArray);
                string foldPath = new String(charArray);
                foldPath = foldPath.Substring(0, foldPath.IndexOf('\0'));
                return foldPath;
            }
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class OpenDlgDir {
            public IntPtr hwndOwner = IntPtr.Zero;
            public IntPtr pidlRoot = IntPtr.Zero;
            public String pszDisplayName = null;
            public String lpszTitle = null;
            public UInt32 ulFlags = 0;
            public IntPtr lpfn = IntPtr.Zero;
            public IntPtr lParam = IntPtr.Zero;
            public int iImage = 0;
        }

        public class DllOpenFileDialog {
            [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
            public static extern IntPtr SHBrowseForFolder([In, Out] OpenDlgDir odd);

            [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
            public static extern bool SHGetPathFromIDList([In] IntPtr pidl, [In, Out] char[] fileName);

            /// <summary>
            /// 获取当前窗口句柄
            /// </summary>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr GetForegroundWindow();
        }
        #endregion


    }
}