using UnityEngine;

public enum ItemUseType
{
    Instant,
    Hold
}


public abstract class ItemDataSO : ScriptableObject
{
    public string itemName;
    public int point;
    public ItemUseType useType;

    [Tooltip("true = 아이템 저장 / false = 닿자마자 효과 적용")]
    public bool isEquippable = true;

    public virtual void OnUseDown(BallItemSystem user) { }
    public virtual void OnUseUp(BallItemSystem user) { }
    public virtual void OnPickup(BallItemSystem user)
    {
        ScoreManager.instance.AddScore(point);
    }
}
