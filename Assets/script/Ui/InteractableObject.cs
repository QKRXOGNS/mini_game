using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour
{
    public GameObject player;  // 플레이어
    public GameObject uiText;  // "F를 누르면 게임이 켜집니다" UI 오브젝트
    public SpriteRenderer spriteRenderer;  // 오브젝트 스프라이트
    private Color originalColor;  // 원래 색상 저장
    private bool isPlayerNear = false;  // 플레이어 근처 여부

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();  // 자동 할당
        }
        
        originalColor = spriteRenderer.color;  // 원래 색 저장
        uiText.SetActive(false);  // 처음에는 UI 숨김
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("게임 시작 이벤트 발생!");
            StartGame();  // 여기에 실제 게임 시작 기능을 추가 가능
        }
    }

    // 플레이어가 범위 안에 들어왔을 때
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            spriteRenderer.color = Color.green;  // 테두리를 빨간색으로 변경
            uiText.SetActive(true);  // UI 메시지 표시
            isPlayerNear = true;
        }
    }

    // 플레이어가 범위를 벗어났을 때
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            spriteRenderer.color = originalColor;  // 원래 색으로 복귀
            uiText.SetActive(false);  // UI 숨김
            isPlayerNear = false;
        }
    }

    void StartGame()
    {
        // 실제 게임 시작 로직을 여기에 추가
        Debug.Log("게임이 시작되었습니다!");
        SceneManager.LoadScene("a1Scene");
    }
}
