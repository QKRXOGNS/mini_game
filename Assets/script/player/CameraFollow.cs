using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // 따라갈 대상 (캐릭터)
    public float smoothSpeed = 5f;  // 카메라 이동 속도
    public Vector3 offset;  // 카메라 위치 오프셋

    void LateUpdate()
    {
        if (target == null) return;

        // 목표 위치 계산 (캐릭터 위치 + 오프셋)
        Vector3 targetPosition = target.position + offset;

        // 부드러운 이동 적용 (Lerp)
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
