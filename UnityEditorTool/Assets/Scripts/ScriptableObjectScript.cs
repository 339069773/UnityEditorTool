using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum IngredientUnit { Spoon, Cup, Bowl, Piece }

[Serializable]
public class CustomData
{
    public string name;
    public int amount = 1;
    public IngredientUnit unit;
}


[CreateAssetMenuAttribute]
public class ScriptableObjectScript : ScriptableObject
{
    public CustomData data;
    public List<CustomData> datas;


}
