using UnityEditor;
using UnityEngine;

// https://catlikecoding.com/unity/tutorials/editor/custom-data/
// https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
// https://www.cnblogs.com/yangrouchuan/p/6698844.html
//The default for one line is 16 pixels. Adding a second line requires 18 additional pixels,
//16 for the second line plus a margin of 2 between then

// IngredientDrawer
[CustomPropertyDrawer(typeof(CustomData))]
public class IngredientDrawer : PropertyDrawer{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
       // position = new Rect(position.x, position.y, position.width, position.height);
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, 30, 16);
        var unitRect = new Rect(position.x + 35, position.y, 50, 16);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"),new GUIContent("Amoun"));
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        var nameRect = new Rect(position.x, position.y+20, position.width - 90, 16);

        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"),GUIContent.none);
         EditorGUI.EndProperty();

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Screen.width < 333 ? (16f + 18f) : 16f;
    }

}
