using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // WASD 입력 받기
        movement.x = Input.GetAxisRaw("Horizontal"); // A, D 키 입력
        movement.y = Input.GetAxisRaw("Vertical");   // W, S 키 입력
    }

    void FixedUpdate()
    {
        // 캐릭터 이동
        rb.velocity = movement.normalized * moveSpeed;
    }
}
