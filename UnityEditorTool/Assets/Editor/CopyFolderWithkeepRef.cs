using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CopyFolderWithkeepRef : Editor
{
    private static readonly string[] InPrefabIgnoreReplaceGUID = {  
            // "*.meta",  
            // "*.mat",  
            // "*.anim",  
            // "*.prefab",  
            // "*.unity",  
            // "*.asset"  
            ".cs",
            ".dll",
        };
    private static readonly string[] SomeFileToReplaceGUID = {  
            ".prefab",
            ".asset",
     };

    [MenuItem(@"BearJTools/Copy/Promotion&Banner")]
    static private void CopyPromotion()
    {
        if (Selection.objects.Length <= 0)
        {
            Debug.LogError("Please Select Some Dic!!");
            return;
        }
        string oldFolderPath = AssetDatabase.GetAssetPath(Selection.objects[0]);
        var GUID_Map = GetGUIDMap(oldFolderPath);
        string newFolderPath = oldFolderPath + "_Copy";
        while (Directory.Exists(newFolderPath))
        {
            newFolderPath = newFolderPath + "_Copy";
        }
        UtilFile.CopyDirectory(oldFolderPath, newFolderPath);
        SetNewGUID2Meta(GUID_Map, newFolderPath);
        //AssetDatabase.Refresh(); spine 材质球复制问题，新文件夹guid log问题
        ReplaceOldGUIDRef(GUID_Map, newFolderPath);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
    static private void WritePrefab(string path,string content)
    {
        FileInfo fileInfo = new FileInfo(path);
        var original = fileInfo.Attributes;
        fileInfo.Attributes = FileAttributes.Normal;
        File.WriteAllText(path, content);
        fileInfo.Attributes = original;
    }
    static private bool IsReplaceFileType(string filePath) {
        for (int i = 0; i < SomeFileToReplaceGUID.Length; i++)
        {
            if (filePath.EndsWith(SomeFileToReplaceGUID[i]))
            {
                return true;
            }
        }
        return false;
    }
    static private void ReplaceOldGUIDRef(Dictionary<string, string> dic,string folder_path) {
        string[] fileName = Directory.GetFiles(folder_path,"*",SearchOption.AllDirectories);
        foreach (string filePath in fileName)
        {
            //Debug.Log(filePath);
            FileInfo fileInfo = new FileInfo(filePath);
            if (IsReplaceFileType(filePath))
            {
                string contents = File.ReadAllText(filePath); 
                //Debug.Log(contents);
                IEnumerable<string> guids = UtilGuids.GetGuids(contents);
                foreach (var item in guids)
                {
                    if (dic.ContainsKey(item))
                    {
                        contents = contents.Replace(item, dic[item]);
                        Debug.Log(string.Format("=Success !=In {0}=\n old={1}\n new={2} ", fileInfo.Name, AssetDatabase.GUIDToAssetPath(item), (dic[item])));
                    }
                    else {
                        Debug.LogWarning("=Replace Fail != No Key Match :" + UtilGuids.GetFildNameByGUID(item));
                    }
                }
                WritePrefab(filePath, contents);
            }
        }
    }
    static private void SetNewGUID2Meta(Dictionary<string, string> dic,string folder_path)
    {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        string[] fileName = Directory.GetFiles(folder_path, "*.meta",SearchOption.AllDirectories);
        foreach (string filePath in fileName)
        {
            string contents = File.ReadAllText(filePath);
            IEnumerable<string> get_guild_in_prefab = UtilGuids.GetGuids(contents);
            foreach (var item in get_guild_in_prefab)
            {
                if (dic.ContainsKey(item))
                {
                    contents = contents.Replace(item, dic[item]);
                    WritePrefab(filePath, contents);
                }
            }
        }
    }
    static private Dictionary<string, string> GetGUIDMap(string folder_path)
    {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        string[] fileName = Directory.GetFiles(folder_path,"*",SearchOption.AllDirectories);
        foreach (string filePath in fileName)
        {
            string id = AssetDatabase.AssetPathToGUID(filePath);
            if (!String.IsNullOrEmpty(id))
            {
                string newGuid = Guid.NewGuid().ToString("N");
                if (!ret.ContainsKey(id))
                {
                    ret.Add(id, newGuid);
                }
            }
        }
        return ret;
    }

    public class UtilGuids
    {
        static public string GetFildNameByGUID(string guid)
        {
            if (String.IsNullOrEmpty(guid))
            {
                return string.Empty;
            }
            var file_path = AssetDatabase.GUIDToAssetPath(guid);
            try
            {
                FileInfo fileInfo = new FileInfo(file_path);
                return fileInfo.Name;
            }
            catch (Exception)
            {
                return string.Empty;
                throw;
            }
        }
        public static IEnumerable<string> GetGuids(string text)
        {
            const string guidStart = "guid: ";
            const int guidLength = 32;
            int textLength = text.Length;
            int guidStartLength = guidStart.Length;
            List<string> guids = new List<string>();

            int index = 0;
            while (index + guidStartLength + guidLength < textLength)
            {
                index = text.IndexOf(guidStart, index, StringComparison.Ordinal);
                if (index == -1)
                    break;

                index += guidStartLength;
                string guid = text.Substring(index, guidLength);
                index += guidLength;

                if (IsGuid(guid))
                {
                    guids.Add(guid);
                }
            }
            return guids;
        }
        private static bool IsGuid(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (
                    !((c >= '0' && c <= '9') ||
                      (c >= 'a' && c <= 'z'))
                    )
                    return false;
            }

            return true;
        }
        private static string MakeRelativePath(string fromPath, string toPath)
        {
            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath;
        }
    }
    public class UtilFile
    {
        public static void CopyDirectory(string sourceDirectory, string destDirectory)
        {
            //判断源目录和目标目录是否存在，如果不存在，则创建一个目录
            if (!Directory.Exists(sourceDirectory))
            {
                Directory.CreateDirectory(sourceDirectory);
            }
            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }
            //拷贝文件
            CopyFile(sourceDirectory, destDirectory);
            //拷贝子目录       
            //获取所有子目录名称
            string[] directionName = Directory.GetDirectories(sourceDirectory);
            foreach (string directionPath in directionName)
            {
                //根据每个子目录名称生成对应的目标子目录名称
                string directionPathTemp = Path.Combine(destDirectory, directionPath.Substring(sourceDirectory.Length + 1));// destDirectory + "\\" + directionPath.Substring(sourceDirectory.Length + 1);
                                                                                                                            //递归下去
                CopyDirectory(directionPath, directionPathTemp);
            }
        }
        public static void CopyFile(string sourceDirectory, string destDirectory)
        {
            //获取所有文件名称
            string[] fileName = Directory.GetFiles(sourceDirectory);
            foreach (string filePath in fileName)
            {
                //根据每个文件名称生成对应的目标文件名称
                string filePathTemp = Path.Combine(destDirectory, filePath.Substring(sourceDirectory.Length + 1));// destDirectory + "\\" + filePath.Substring(sourceDirectory.Length + 1);
                //若不存在，直接复制文件；若存在，覆盖复制
                if (File.Exists(filePathTemp))
                {
                    File.Copy(filePath, filePathTemp, true);
                }
                else
                {
                    File.Copy(filePath, filePathTemp);
                }
            }
        }
    }

}
