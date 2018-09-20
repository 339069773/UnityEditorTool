using UnityEngine;

//对一个MonoBehaviour的子类使用这个属性，那么在同一个GameObject上面，最多只能添加一个该Class的实例。
//[DisallowMultipleComponent]

/*
[ExecuteInEditMode]
 
默认状态下，MonoBehavior中的Start，Update，OnGUI等方法，需要在Play的状态下才会被执行。
这个属性让Class在Editor模式（非Play模式）下也能执行。
但是与Play模式也有一些区别。
例如：
Update方法只在Scene编辑器中有物体产生变化时，才会被调用。
OnGUI方法只在GameView接收到事件时，才会被调用。
 */

public class ToolsInMono : MonoBehaviour {

/// <summary>
/// Reset 不加 ContextMenu 也会作为菜单选项
/// 可声明为 public 在其他Mono中调用
/// </summary>
    private void Reset()
    {
        Debug.Log("Reset");
    }

    /// <summary>
    /// 在Inspector的ContextMenu中增加选项。
    /// </summary>
    [ContextMenu("DoSomeThing")]
    void DoSomething()
    {
        Debug.Log("DoSomething in"+gameObject.name);
    }

    /// <summary>
    /// 在Inspector上面对变量追加一个右键菜单，并执行指定的函数。
    /// </summary>
    [ContextMenuItem("Reset","ResetName")]
    [Header("魔法值")]
    [Tooltip("This year is 2015!")]
    public string name = "Default";
    void ResetName()
    {
        name = "Default";
    }

    /*
     [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // Copy the source Render Texture to the destination,
        // applying the material along the way.
        Graphics.Blit(src, dest, mat);
    }
     */
}
/*
 * 参考
 * https://blog.csdn.net/spring_shower/article/details/48708337
 * 
 * 
 * 
 */