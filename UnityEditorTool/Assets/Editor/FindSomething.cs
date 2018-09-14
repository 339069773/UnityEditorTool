using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;

namespace Assets.Editor
{
    public class FindSomething:UnityEditor.Editor
    {
        /// <summary>
        /// ...YouProjectName/Assets
        /// </summary>
        public static string AssetsPath = Application.dataPath;
        /// <summary>
        /// Assets 的上层目录 是可以用 ../ 拼到路径中的
        /// </summary>
        public static string AssetsPathUpFolder = Application.dataPath+"/../";

        static readonly StringBuilder LogStringBuilder = new StringBuilder();
        #region 通过名称后缀 搜索 Assets 下面的文件        [MenuItem(@"BearJ Find/Find By Way 1")]
        public static void FindByWay1()
        {
            string fileName = AssetsPathUpFolder + "FindByWay1LogInfo.txt";
            FileStream logfile = File.Create(fileName);
            StreamWriter sw = new StreamWriter(logfile);
            //查找 Assets 下面的 *.prefab 文件 
            DirectoryInfo direction = new DirectoryInfo(Application.dataPath);
            FileInfo[] filesPrefab = direction.GetFiles("*.prefab",SearchOption.AllDirectories);
            for(int idx = 0;idx < filesPrefab.Length;++idx)
            {
                string prefabPath = FormatPath(filesPrefab[idx].FullName);
                LogStringBuilder.AppendLine(prefabPath);

                EditorUtility.DisplayProgressBar("查找Prefab：",prefabPath + " " + idx + "/" + filesPrefab.Length,progress :(idx * 1.0f / filesPrefab.Length));

                sw.WriteLine(LogStringBuilder.ToString());
                LogStringBuilder.Remove(0,LogStringBuilder.Length);
            }
            sw.Close();
            EditorUtility.ClearProgressBar();
            ShowDialogueOpenResult("fileName");
        }
        private static string FormatPath(string path)
        {
            return path.Substring(path.IndexOf("Assets", StringComparison.Ordinal));
        }
        #endregion

        /// <summary>
        /// 展示对话框，点击确定打开指定路径文件于文件夹中
        /// </summary>
        /// <param name="path"></param>
        public static void ShowDialogueOpenResult(string path)
        {
            if(EditorUtility.DisplayDialog("Prefab信息导出","导出成功！","打开导出信息文件夹"))
            {
                System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo();
                processStartInfo.FileName = "explorer.exe";  //资源管理器
                string newPath = path.Replace('/','\\');
                processStartInfo.Arguments = "/select," + newPath;
                //可启动系统应用，此处启动资源管理器
                System.Diagnostics.Process.Start(processStartInfo);
            }
        }

        /// <summary>
        /// 改变工程贴图格式 
        /// 改变 .meta 文件也能实现，不过改变之后 Unity 要重新导入 编译很久
        /// 此方法不会重新编译 很快的说
        /// </summary>
        private static void ChangeTextureSize(string texturePath)
        {
            //AssetDatabase.LoadAssetAtPath 只能读取路径以 Assets 开头的文件路径
            Texture2D t2D = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath,typeof(Texture2D));
            if(t2D == null)
            {
                Debug.LogError("Error  Texture2D 为空！" + texturePath);
                return;
            }

            if(t2D.height != t2D.width)
            {
                Debug.LogError("Error    非正方形贴图:" + texturePath);
            }
//            TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(t2D));
            TextureImporter ti = (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(t2D));
            int origin = t2D.height;
            //原 512 以上的都压缩为 512
            if(origin > 512)
            {
                ti.maxTextureSize = 512;
            }
            //原 128 以上的都压缩为 128
            else if(origin > 128)
            {
                ti.maxTextureSize = 128;
            }
            //原 32 以上的都压缩为 32
            else
            {
                ti.maxTextureSize = 32;
            }
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(t2D));
        }

    }
}
