using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BearJPrefabHelp : Editor {

   // 代码实现拖拽赋值 绑定Buttong点击事件
      public static bool AddPlayAudioToGameObj(GameObject ins)
    {
        var btns = ins.transform.GetComponentsInChildren<Button>(true);
        bool change = false;
        foreach (var btn in btns)
        {
            if (!btn.transform.GetComponent<Image>())
            {
                var pa = btn.gameObject.AddComponent<Image>();
                pa.raycastTarget = true;
                UnityEvent BigExplosionEvent = new UnityEvent();
                var targetInfo = UnityEvent.GetValidMethodInfo(pa, "Play", new Type[0]);// "Play" 为目标组件中需要绑定的共有方法
                UnityAction methodDelegate = Delegate.CreateDelegate(typeof(UnityAction), pa, targetInfo) as UnityAction;
                UnityEventTools.AddPersistentListener(btn.onClick, methodDelegate);
                change = true;
            }
        }
        return change;
    }

    //Hierartchy 右键
    //[MenuItem("GameObject/Add Button Audio", priority = -97)]
    

    [MenuItem(@"BearJTools/Prefab Tools/Change Prefabs")]
    static void ChangePrefabs()
    {
        List<string> paths = new List<string> {
            "Assets/AssetsPackage/UI/VegasClan/prefab",
            "Assets/AssetsPackage/UI/VegasClan_theme/prefab",
        };
        foreach (var item in paths)
        {
            DoLogicToSelectPrefabs(item, ChangePrefabLogic);
        }
    }
    static bool ChangePrefabLogic(GameObject target)
    {
        return false;
    }

    //Asset下目录 批量修改 prefab
    static void DoLogicToSelectPrefabs(string target_path, Func<GameObject ,bool> action)
    {
        //目录
        if (Directory.Exists(target_path))
        {
            var allprefab = Directory.GetFiles(target_path, "*.prefab", SearchOption.AllDirectories);
            foreach (var item in allprefab)
            {
                var target = AssetDatabase.LoadAssetAtPath<GameObject>(item);
                var ins = PrefabUtility.InstantiatePrefab(target) as GameObject;
                bool change = action(ins);
                if (change)
                {
                    PrefabUtility.ApplyPrefabInstance(ins, InteractionMode.UserAction);
                }
                DestroyImmediate(ins);
            }
        }
        //文件
        else
        {
            if (target_path.EndsWith(".prefab"))
            {
                var target = AssetDatabase.LoadAssetAtPath<GameObject>(target_path);
                var ins = PrefabUtility.InstantiatePrefab(target) as GameObject;
                bool change = action(ins);
                if (change)
                {
                    PrefabUtility.ApplyPrefabInstance(ins, InteractionMode.UserAction);
                }
                DestroyImmediate(ins);
            }
        }
    }
}
