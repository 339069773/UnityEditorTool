using UnityEditor;
using UnityEngine;


[CustomEditor(typeof (EditorScene))]
public class CustomEditorScene : Editor
{

    public override void OnInspectorGUI()
    {
        EditorScene editorTest = (EditorScene) target;
        editorTest.MRect = EditorGUILayout.RectField("窗口坐标", editorTest.MRect);
        editorTest.MTexture =
            EditorGUILayout.ObjectField("贴图", editorTest.MTexture, typeof (Texture), true) as Texture;
    }

    private void OnSceneGUI()
    {
        EditorScene editorTest = (EditorScene) target;
        Handles.Label(editorTest.transform.localPosition, editorTest.gameObject.name);
        Handles.BeginGUI();

        GUILayout.BeginArea(new Rect(100, 100, 100, 100));
        if (GUILayout.Button("这是一个按钮"))
        {
            Debug.LogError("==" + editorTest.name);
        }
        GUILayout.Label("Des Label");
        GUILayout.EndArea();

        Handles.EndGUI();
    }
}
