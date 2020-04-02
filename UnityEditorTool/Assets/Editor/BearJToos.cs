using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class BearJToos : Editor
{
    [MenuItem(@"BearJTools/ChangePrafab")]
    public static void FindPrefab()
    {
        //string p = "Assets/TestData/GameObject.prefab";
        string file = Application.dataPath + "/TestData/";
        file = "Assets/TestData/";
        //file = "Assets/TestData2/test2.txt";
        //StreamWriter sw =  BearJFileHelp.WirteTxt(file,true);
        //if (sw!=null)
        //{
        //    sw.WriteLine("what a win a!");
        //    sw.Close();
        //}
        var a = BearJFileHelp.GetFileNames(file);
        foreach (var item in a)
        {
            Debug.Log(item);
        }
    }

#if UNITY_2018_3_OR_NEWER
//只能Load以Asset目录开头的文件
    public static GameObject GetPrefabByPath(string assetPath)
    {
        GameObject _prefab = null;
        _prefab = PrefabUtility.LoadPrefabContents(assetPath);
        return _prefab;
    }
#else
    //只能Load以Asset目录开头的文件
    public static GameObject GetPrefabByPath(string assetPath)
    {
        GameObject _prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
        return _prefab;
    }
#endif
    public static void CreatPrefab(string prefabPath)
    {
        if (!prefabPath.EndsWith(".prefab"))
        {
            EditorUtility.DisplayDialog("CreatPrefab Error !",
                prefabPath + " Need  End With .prefab", "OK");
            return;
        }
        //这样创建出来的Prefab图标是白色的，不能拖到场景中，连Transform组件也没有
        PrefabUtility.CreateEmptyPrefab(prefabPath);
    }

    void DisplayProgressBarDemo(){
        EditorUtility.DisplayProgressBar("Title","info a/b",0.2f);
        EditorUtility.ClearProgressBar();
    }

}
