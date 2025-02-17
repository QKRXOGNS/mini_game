using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement; 

public class GoogleSheetManager : MonoBehaviour
{
    public InputField nameInputField;
    public InputField passwordInputField;
    public Text statusText;
    private string googleScriptURL = "https://script.google.com/macros/s/AKfycbyFybZ-x7fDSpSorW2Lux3nRV7YJkwmCYz_5hiDe009AZoGutqGElHrcO1EhKNrCLWUCw/exec"; // 최신 GAS 배포 URL

    public void OnLoginButtonClicked()
    {
        string playerName = nameInputField.text;
        string playerPassword = passwordInputField.text;
        if (!string.IsNullOrEmpty(playerName) && !string.IsNullOrEmpty(playerPassword))
        {
            StartCoroutine(Login(playerName, playerPassword));
        }
    }

    public void OnRegisterButtonClicked()
    {
        string playerName = nameInputField.text;
        string playerPassword = passwordInputField.text;
        if (!string.IsNullOrEmpty(playerName) && !string.IsNullOrEmpty(playerPassword))
        {
            StartCoroutine(Register(playerName, playerPassword));
        }
    }

    IEnumerator Login(string name, string password)
    {
        string url = googleScriptURL + "?action=login&name=" + UnityWebRequest.EscapeURL(name) + "&password=" + UnityWebRequest.EscapeURL(password);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                Debug.Log("로그인 응답: " + response);

                if (response.Contains("\"response\":\"success\""))
                {
                    statusText.text = "로그인 성공!";
                    yield return new WaitForSeconds(1f); // UI 갱신을 위한 짧은 대기
                    SceneManager.LoadScene("SampleScene");
                }
                else if (response.Contains("\"response\":\"fail: incorrect password\""))
                {
                    statusText.text = "비밀번호가 틀렸습니다!";
                }
                else if (response.Contains("\"response\":\"fail: user not found\""))
                {
                    statusText.text = "사용자가 존재하지 않습니다!";
                }
                else
                {
                    statusText.text = "로그인 실패! 응답: " + response;
                }
            }
            else
            {
                statusText.text = "네트워크 오류!";
                Debug.LogError("네트워크 오류: " + request.error);
            }
        }
    }

    IEnumerator Register(string name, string password)
    {
        string url = googleScriptURL + "?action=register&name=" + UnityWebRequest.EscapeURL(name) + "&password=" + UnityWebRequest.EscapeURL(password);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                Debug.Log("회원가입 응답: " + response);

                if (response.Contains("\"response\":\"registered\""))
                {
                    statusText.text = "회원가입 완료! 이제 로그인하세요.";
                }
                else if (response.Contains("\"response\":\"exists\""))
                {
                    statusText.text = "이미 존재하는 이름입니다.";
                }
                else
                {
                    statusText.text = "회원가입 실패! 응답: " + response;
                }
            }
            else
            {
                statusText.text = "네트워크 오류!";
                Debug.LogError("네트워크 오류: " + request.error);
            }
        }
    }
}
