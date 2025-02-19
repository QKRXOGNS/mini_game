using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private float prevSpeed = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // WASD 또는 방향키 입력 받기
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 이동 방향 정규화
        movement.Normalize();

        // 방향 전환
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false; 
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        // Speed 값 계산
        float speedValue = movement.sqrMagnitude;

        // Speed 값이 이전 값과 다를 때만 업데이트
        if (Mathf.Abs(speedValue - prevSpeed) > 0.01f)
        {
            animator.SetFloat("Speed", speedValue);
            prevSpeed = speedValue; // 이전 값 업데이트
        }

        // 이동하지 않으면 Rigidbody 속도 제거
        if (speedValue == 0)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        // Rigidbody2D를 사용하여 이동
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    bool IsOnTilemap(Vector2 targetPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(targetPos, Vector2.zero, 0.1f, LayerMask.GetMask("Tilemap"));
        return hit.collider != null;
    }
}
