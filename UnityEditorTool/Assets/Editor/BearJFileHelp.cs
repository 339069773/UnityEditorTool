using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class BearJFileHelp{

    /// <summary>
    /// 使用 Application.dataPath(XX.Assets) 或者 直接 Assets/XX/ 均可
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    public static bool CreatFile(string filePath,string fileName)
    {
        Debug.LogWarning("CreatFile: "+ filePath + fileName);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        if (!File.Exists(filePath+fileName))
        {
            var fs = File.Create(filePath + fileName);
            fs.Close();
            AssetDatabase.Refresh();
            return true;
        }
        return false;
    }

    public static void CreatDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogWarning("CreatDirectory: " + path);
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
    }
/// <summary>
///  StreamWriter sw =  BearJFileHelp.WirteTxt(file,true);
///  sw.WriteLine(str); sw.Close();
/// </summary>
/// <param name="path"></param>
/// <returns></returns>
public static StreamWriter WirteTxt(string path,bool creatWhenNotExists = false) {
        if (!path.EndsWith(".txt"))
        {
            return null;
        }
        Debug.LogWarning("WirteTxt: " + path);
      //  return File.CreateText(path);

        if (!File.Exists(path))
        {
            if (creatWhenNotExists)
            {
                string file_path = Path.GetDirectoryName(path);
                Debug.LogWarning("GetDirectoryName: " + file_path);
                CreatDirectory(file_path);
            }
            else {
               return null;
            }
        }
        return File.CreateText(path); ;
    }

public static void Copy() {
}

    /// <summary>
    /// 获取目录下所有文件名
    /// </summary>
    /// <param name="directory"></param>
    public static List<string> GetFileNames(string path,string searchPattern, SearchOption option) {
        if (!Directory.Exists(path))
        {
            return null;
        }
        var ret = Directory.GetFiles(path,searchPattern, option)
        return ret;
    }

    /*
    如何确定一个C#路径字符串是表示目录还是文件
    使用Directory.Exists或File.Exist方法，如果前者为真，则路径表示目录；如果后者为真，则路径表示文件
    上面的方法有个缺点就是不能处理那些不存在的文件或目录。这时可以考虑使用Path.GetFileName方法获
    得其包含的文件名，如果一个路径不为空，而文件名为空那么它表示目录，否则表示文件；
         */
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileFullPath"></param>
    /// <returns></returns>
    public static string GetFileName(string fileFullPath) {
        return Path.GetDirectoryName(fileFullPath);

        if (Directory.Exists(fileFullPath))
        {
            //is directory
        }
        else {
            string fileName = Path.GetFileName(fileFullPath);
            if (fileName != null)
            {
                //is file 
            }
            else {
                 //is dire not exits
            }
        }
        if (File.Exists(fileFullPath))
        {
            //is file
        }
        else {
            string fileName = Path.GetFileName(fileFullPath);
            if (fileName != null)
            {
                //is file 
            }
            else
            {
                //is dire not exits
            }
        }

    }
}
