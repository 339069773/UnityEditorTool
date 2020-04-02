using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangeRenderQueue : Editor
{
    [MenuItem("BearJTools/ChangeObjRenderQueue", true)]
    public static bool SelectGameObject()
    {
        var selectedObject = Selection.activeObject;
        return selectedObject is GameObject;
    }
    /// <summary>
    /// 有使用条件的按钮
    /// </summary>
    [MenuItem(@"BearJTools/ChangeObjRenderQueue")]
    public static void ChangeObjRenderQueue()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.Deep);
        for (int i = 0; i < transforms.Length; i++)
        {
            Debug.Log(transforms[i].name);
            var t = transforms[i];
            var canvas = t.GetComponent<Canvas>();
            if (canvas&& canvas.overrideSorting)
            {
                canvas.sortingOrder += 10;
            }
            var particle = t.GetComponent<ParticleSystem>();
            if (particle)
            {
                //particle.customData
            }

        }
    }
}
