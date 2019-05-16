using ClockStone;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BearJToos : Editor
{
    [MenuItem(@"BearJTools/ChangePrafab")]
    public static void FindPrefab()
    {
        GetPrefabPath();
    }
    public static void ChangePrefab(string Path) {
        string path = "Assets/AAAMyTest/GameObject.prefab";
        path = Path;
        GameObject root = PrefabUtility.LoadPrefabContents(path);
        if (root)
        {
            var _StopMusics = root.GetComponentsInChildren<StopMusic>();
            for (int i = 0; i < _StopMusics.Length; i++)
            {
                var _StopMusic = _StopMusics[i];
                if (_StopMusic.triggerEvent == AudioTriggerBase.EventType.OnEnable)
                {
                    _StopMusic.triggerEvent = AudioTriggerBase.EventType.Start;
                    _StopMusic.changeTriggerStartToEnable = true;
                    _StopMusic.fadeOutTime = 0;
                }
            }
            bool reslut = false;
            PrefabUtility.SaveAsPrefabAsset(root, path, out reslut);
            PrefabUtility.UnloadPrefabContents(root);
            if (!reslut)
            {
                Debug.LogWarning("Fail Merge Prefab");
            }
            else
            {
                Debug.Log("Success "+ Path);
            }
        }
    }
    public static void GetPrefabPath() {
        List<string> path = MyFindComponent.CheckSlider();
        for (int i = 0; i < path.Count; i++)
        {
            Debug.Log("预制路径为：" + path[i]);
            ChangePrefab(path[i]);
        }
    }

}
