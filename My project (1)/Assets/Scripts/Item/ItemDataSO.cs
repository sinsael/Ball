using UnityEngine;

public abstract class ItemDataSO : ScriptableObject
{
    public string itemName;
    public int point;
    public bool isEffect;

    public abstract void Effect(Ball user);
    //1
}
