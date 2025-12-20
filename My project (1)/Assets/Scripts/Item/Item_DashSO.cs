using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Item/Dash")]
public class Item_DashSO : ItemDataSO
{
    [Header("대쉬 파워")]
    public float dashForce = 10f;

    [Header("줌 연출 설정")]
    [Tooltip("줌 인")]
    public float zoomAmount = 3f; // 줌인 기능
    [Tooltip("오프셋")]
    public Vector3 sideOffset;
    [Tooltip("오프셋 바뀌는 시간")]
    public float offSetTime = 1.0f;
    [Tooltip("줌 인까지 걸리는 시간")]
    public float zoomInTime = 0.1f; // 줌 인까지 얼마나 걸리는지
    [Tooltip("줌 아웃까지 걸리는 시간")]
    public float zoomOutTime = 0.5f; // 줌 아웃까지 얼마나 걸리는지
    [Tooltip("슬로우 모션")]
    public float slowMotion = 0.5f;

    public override void OnUseDown(BallItemSystem user)
    {
        user.cameraController.SetZoom(zoomAmount, zoomInTime, sideOffset, offSetTime);


        Time.timeScale = slowMotion;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (user.crosshairUI != null)
        {
            user.crosshairUI.SetActive(true);
        }
    }

    public override void OnUseUp(BallItemSystem user)
    {
        if (user.movement != null && user.cameraController != null)
        {
            Vector3 aimDir = user.cameraController.transform.forward;
            user.movement.ApllyExternalForce(aimDir, dashForce);
        }

        user.cameraController.ResetZoom(zoomOutTime,zoomOutTime);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        if (user.crosshairUI != null)
        {
            user.crosshairUI.SetActive(false);
        }
    }
}
