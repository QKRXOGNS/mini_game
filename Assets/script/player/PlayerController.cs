using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    public LayerMask tilemapLayer;

    private float prevSpeed = -1f; // 🔹 이전 Speed 값 저장 (초기값 -1)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 🔹 중력 제거
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // 🔹 WASD 입력 받기
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 🔹 이동 방향 정규화
        movement = movement.normalized;

        // 🔹 방향 전환 (A → 왼쪽 / D → 오른쪽)
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false; 
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        // 🔹 Speed 값 계산
        float speedValue = movement.sqrMagnitude;

        // 🔹 **Speed 값이 이전 값과 다를 때만 업데이트**
        if (Mathf.Abs(speedValue - prevSpeed) > 0.01f)
        {
            animator.SetFloat("Speed", speedValue);
            prevSpeed = speedValue; // 🔹 이전 값 업데이트
        }

        // 🔹 이동하지 않으면 Rigidbody 속도 제거
        if (speedValue == 0)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        // 🔹 이동할 위치 계산
        Vector2 nextPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // 🔹 이동할 위치가 발판 위인지 확인
        if (IsOnTilemap(nextPosition))
        {
            rb.MovePosition(nextPosition);
        }
        else
        {
            rb.velocity = Vector2.zero; // 🔹 이동 불가능한 경우 속도를 완전히 0으로
        }
    }

    bool IsOnTilemap(Vector2 targetPos)
    {
        // 🔹 Raycast를 사용하여 Tilemap 레이어 확인
        RaycastHit2D hit = Physics2D.Raycast(targetPos, Vector2.zero, 0.1f, tilemapLayer);
        return hit.collider != null;
    }
}
