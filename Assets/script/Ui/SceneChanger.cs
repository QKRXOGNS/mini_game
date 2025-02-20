using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 관련 네임스페이스 추가

public class SceneChanger : MonoBehaviour
{
    public string sceneName; // 인스펙터에서 설정할 씬 이름
    public Button changeSceneButton; // 씬 변경 버튼

    void Start()
    {
        // 버튼 클릭 이벤트에 ChangeScene 메서드 추가
        if (changeSceneButton != null)
        {
            changeSceneButton.onClick.AddListener(ChangeScene);
        }
        else
        {
            Debug.LogWarning("변경할 버튼이 설정되지 않았습니다.");
        }
    }

    void ChangeScene()
    {
        // 씬 이름이 비어있지 않은 경우에만 씬 변경
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("씬 이름이 설정되지 않았습니다.");
        }
    }
} 