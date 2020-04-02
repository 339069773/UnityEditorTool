using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

    public class MyFindComponent : Editor
{
        private static string _strUiPerfabPath;

        private static List<string> _prefabList;

        private static List<string> _resPrefabs;

        private static string _sliderType;

    public static string AssetsPathUpFolder = Application.dataPath + "/../";
    //static readonly StringBuilder LogStringBuilder = new StringBuilder();
    private static void Init()
    {
        _strUiPerfabPath = "Assets/AAAMyTest/";
        _strUiPerfabPath = "Assets/AssetsPackage/";
        _strUiPerfabPath = "Assets/AssetsPackage/Themes/Theme50002/";
        _sliderType = "UnityEngine.UI.Slider";
        _sliderType = "m_Script: {fileID: 11500000, guid: 5bc1b7cfbcb2a3d45916630aeaa6b9db, type: 3}\n  m_Name: \n  m_EditorClassIdentifier: \n  triggerEvent: 5";


        if (_prefabList == null)
        {
            _prefabList = new List<string>();
        }
        _prefabList.Clear();

        if (_resPrefabs == null)
        {
            _resPrefabs = new List<string>();
        }
        _resPrefabs.Clear();
    }


    [MenuItem("Tools/Find My Component")]
        public static List<string>  CheckSlider()
    {
            Init();
            GetAllPrefabs(_strUiPerfabPath);
            CheckPrefab();
        //    string fileName = AssetsPathUpFolder + "FindByWay1LogInfo.txt";
        //    FileStream logfile = File.Create(fileName);
        //    StreamWriter sw = new StreamWriter(logfile);

        //    for (int i = 0; i < _resPrefabs.Count; i++)
        //        {
        //            Debug.Log("预制路径为：" + _resPrefabs[i]);
        //            LogStringBuilder.AppendLine(_resPrefabs[i]);
        //    }
        //sw.WriteLine(LogStringBuilder.ToString());
        //sw.Close();
        //                ShowDialogueOpenResult("fileName");
        return _resPrefabs;
    }
    public static void ShowDialogueOpenResult(string path)
    {
        if (EditorUtility.DisplayDialog("Prefab信息导出", "导出成功！", "打开导出信息文件夹"))
        {
            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo();
            processStartInfo.FileName = "explorer.exe";  //资源管理器
            string newPath = path.Replace('/', '\\');
            processStartInfo.Arguments = "/select," + newPath;
            //可启动系统应用，此处启动资源管理器
            System.Diagnostics.Process.Start(processStartInfo);
        }
    }

        /// <summary>
        /// 检测prefab
        /// </summary>
        private static void CheckPrefab()
        {
            if (_prefabList == null || _prefabList.Count == 0) return;

            _resPrefabs.Clear();
            for (int i = 0; i < _prefabList.Count; i++)
            {
                EditorUtility.DisplayProgressBar("搜索预制体", "搜索中" + _prefabList[i], i / _prefabList.Count);
                FileStream fs = new FileStream(_prefabList[i], FileMode.Open, FileAccess.Read);
                byte[] buff = new byte[fs.Length];
                fs.Read(buff, 0, (int)fs.Length);
                string strText = Encoding.Default.GetString(buff);

                if (strText.Contains(_sliderType))
                {
                    _resPrefabs.Add(_prefabList[i]);
                }
                fs.Close();
            }
            EditorUtility.ClearProgressBar();
        }


        /// <summary>
        /// 获取路径下全部的prefab
        /// </summary>
        /// <param name="strPrefabsPath"></param>
        private static void GetAllPrefabs(string strPrefabsPath)
        {
            //获取路径目录
            var dirArr = Directory.GetDirectories(strPrefabsPath);
            for (int i = 0; i < dirArr.Length; i++)
            {
                var pathArr = GetFiles(dirArr[i]);
                for (int j = 0; j < pathArr.Length; j++)
                {
                    var filePath = pathArr[j];
                    _prefabList.Add(filePath);
                }
            }

            var paths = GetFiles(strPrefabsPath);
            for (int i = 0; i < paths.Length; i++)
            {
                var filePath = paths[i];
                _prefabList.Add(filePath);
            }

            Debug.Log("AllPrefabCount.." + _prefabList.Count);

        }

        /// <summary>
        ///  获取目录下的所有对象路径，except mate
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="recursive">是否递归获取</param>
        /// <returns></returns>
        private static  string[] GetFiles(string path, bool recursive = true)
        {
            var resultList = new List<string>();

            var dirArr = Directory.GetFiles(path, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirArr.Length; i++)
            {
                if (Path.GetExtension(dirArr[i]) != ".meta")
                {
                    resultList.Add(dirArr[i].Replace('\\', '/'));
                }
            }

            return resultList.ToArray();
        }

    [ExecuteInEditMode]
      [MenuItem("Tools/RecordPoint Add Flame")]
      private static void RecordPointAddFlame()
      {

        }



    }
