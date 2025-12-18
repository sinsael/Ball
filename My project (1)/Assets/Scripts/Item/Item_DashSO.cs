using UnityEngine;

public class Item_DashSO : ItemDataSO
{
    [Header("대쉬 파워")]
    public float dashForce = 10f;

    [Header("줌 연출 설정")]
    public float zoomAmount = 3f;
    public float zoomInTime = 0.1f;
    public float zoomOutTime = 0.5f;

    public override void OnUseDown(BallItemSystem user)
    {
        user.cameraController.SetZoom(zoomAmount, zoomInTime);

        user.crosshairUI.SetActive(true);
    }

    public override void OnUseUp(BallItemSystem user)
    {
        if (user.movement != null && user.cameraController != null)
        {
            Vector3 aimDir = user.cameraController.transform.forward;
            user.movement.ApllyExternalForce(aimDir, dashForce);
        }

        user.cameraController.ResetZoom(zoomOutTime);
        user.crosshairUI.SetActive(false);
    }
}
