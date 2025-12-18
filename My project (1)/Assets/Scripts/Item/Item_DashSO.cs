using UnityEngine;

public class Item_DashSO : ItemDataSO
{
    public float dashForce = 10f;
    public override void Effect(BallItemSystem user)
    {
        Rigidbody rb = user.movement.GetComponent<Rigidbody>();

        Vector3 dashDirection = rb.linearVelocity.normalized;
        if(dashDirection == Vector3.zero)
        {
            dashDirection = user.transform.forward;

            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
        }
    }
}
