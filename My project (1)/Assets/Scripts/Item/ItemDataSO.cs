using UnityEngine;

public enum ItemUseType
{
    Instant,
    Hold
}


public class ItemDataSO : ScriptableObject
{
    [Header("아이템 상세설정")]
    [Tooltip("이름")]
    public string itemName;
    [Tooltip("점수")]
    public int point;
    [Tooltip("홀드 시간")]
    public float maxHoldTime = 2.0f;
    public ItemUseType useType; // 아이템 타입

    [Header("True = 아이템 저장 / false = 닿자마자 효과 적용")]
    [Tooltip("true = 아이템 저장 / false = 닿자마자 효과 적용")]
    public bool isEquippable = true;

    public virtual void OnUseDown(BallItemSystem user) { }
    public virtual void OnUseUp(BallItemSystem user) { }
    public virtual void OnPickup(BallItemSystem user)
    {
        ScoreManager.instance.AddScore(point);
    }
}
