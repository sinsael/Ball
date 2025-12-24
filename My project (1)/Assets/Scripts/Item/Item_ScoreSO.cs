using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Score", menuName = "Item/Scroe" )]
public class Item_ScoreSO : ItemDataSO
{

    public override void OnPickup(BallItemSystem user)
    {
        base.OnPickup(user);
    }
}
