using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;

public class MyTools:Editor
{
#region 勾选开关按钮
const string CheckMenu = "BearJTools/SetCheckMenu";
    public static bool Flag = false;
    [MenuItem(CheckMenu)]
    public static void SetCheckMenu()
    {
        bool flag = Menu.GetChecked(CheckMenu);
        Flag = !flag;
        Menu.SetChecked(CheckMenu,Flag);
    }

#endregion

    /// <summary>
    /// Hierarchy 选择对象进行相关操作
    /// </summary>
    [MenuItem(@"BearJTools/Select GameObj _%#_W")]
    public static void SetGameObjDepthReduce()
    {
        String name = "common_cefanyexuanzhong";
        Transform[] transforms = Selection.GetTransforms(SelectionMode.Deep);
        for(int i = 0;i < transforms.Length;i++)
        {
            //TODO
            Debug.Log(transforms[i].gameObject.name);
        }
    }

    #region  有使用条件限制的按钮
/// <summary>
/// 校验功能可用 返回 true 为可用
/// </summary>
/// <returns></returns>
    [MenuItem("BearJTools/ConditionSelect _%#_Q",true)]
    public static bool CheckSetDepth()
    {
        var selectedObject = Selection.activeObject;
        return selectedObject is GameObject;
    }
    /// <summary>
    /// 有使用条件的按钮
    /// </summary>
    [MenuItem(@"BearJTools/ConditionSelect _%#_Q")]
    public static void SetGameObjDepthAdd()
    {
        string logFile = Application.dataPath + "/AAAAAAAAAAA/FS";
        DirectoryInfo fileInfo = new DirectoryInfo(logFile);
        Debug.LogError(fileInfo.Name);
        FileInfo[] fileInfos = fileInfo.GetFiles();
        StringBuilder sb = new StringBuilder();
        for(int i = 0;i < fileInfos.Length;i++)
        {
            if(fileInfos[i].Name.Contains(".meta"))
                continue;
            sb.AppendLine(fileInfos[i].Name);
        }
        FileStream fs = new FileStream(logFile + "/AllSpritesName.txt",FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(sb.ToString());
        sw.Flush();
        sw.Close();
        fs.Close();
    }
    #endregion

    #region 组件拷贝粘贴
    /// <summary>
    /// for ex  Transform
    /// replace youself component
    /// </summary>
    static Transform _transform;
    static GameObject _go;
    /// <summary>
    /// 赋值组件
    /// </summary>
    [MenuItem(@"BearJTools/CopyComponent _%#_C")]
    public static void CopyComponent()
    {
        _transform = Selection.activeGameObject.GetComponent<Transform>();
        _go = Selection.activeGameObject;
    }

    /// <summary>
    /// 粘贴组价
    /// </summary>
    [MenuItem(@"BearJTools/PaseComponent _%#_V")]
    public static void PaseComponent()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.Deep);
        for(int j = 0;j < transforms.Length;j++)
        {
                Transform targeTransform = transforms[j].gameObject.transform;
                UnityEditorInternal.ComponentUtility.CopyComponent(_transform);
                Component componentOld = targeTransform.GetComponent<Transform>();
                if(!componentOld)
                {
                    UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targeTransform.gameObject);
                }
                else
                {
                    UnityEditorInternal.ComponentUtility.PasteComponentValues(componentOld);
                }
            }
        }
#endregion



    }

