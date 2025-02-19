using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Threading.Tasks;

public class LobbyGameManager : MonoBehaviour
{
    public Text scoreDisplayText; // 점수를 표시할 텍스트
    private string googleScriptURL = "https://script.google.com/macros/s/AKfycbwqZaVk-oQuw62ZBCYCsilrUnED6w2NpcbXGH44T9nLU9l4_6J3WRBCOf2ESrLgcpMTMg/exec"; // 최신 GAS 배포 URL

    private async void Start()
    {
        await GetTopScores();
    }

    private async Task GetTopScores()
    {
        string url = googleScriptURL + "?action=getTopScores"; // 스프레드시트에서 점수를 가져오는 URL
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            Debug.Log("Sending request to: " + url);
            
            // 비동기 요청
            var operation = request.SendWebRequest();

            // 요청이 완료될 때까지 대기
            while (!operation.isDone)
            {
                await Task.Yield(); // 메인 스레드를 차단하지 않도록 대기
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                Debug.Log("점수 응답: " + response);
                ParseScores(response);
            }
            else
            {
                Debug.LogError("네트워크 오류: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class ScoreData
    {
        public string name;
        public int score;
    }

    // 응답을 감싸는 클래스
    [System.Serializable]
    public class ResponseData
    {
        public string response; // 실제 점수 데이터가 포함된 필드
    }

    [System.Serializable]
    public class ScoreDataList
    {
        public ScoreData[] scores; // ScoreData 배열을 포함하는 클래스
    }

    private void ParseScores(string jsonResponse)
    {
        Debug.Log("Parsing scores from response: " + jsonResponse);

        // JSON 응답에서 "response" 필드 추출
        var responseData = JsonUtility.FromJson<ResponseData>(jsonResponse);
        
        // null 체크
        if (responseData == null || string.IsNullOrEmpty(responseData.response))
        {
            Debug.LogError("점수 데이터가 null입니다. JSON 응답: " + jsonResponse);
            return; // 오류 발생 시 메서드 종료
        }

        Debug.Log("Extracted response: " + responseData.response);

        // 실제 점수 데이터 파싱
        var scoreDataList = JsonUtility.FromJson<ScoreDataList>("{\"scores\":" + responseData.response + "}");

        // null 체크
        if (scoreDataList == null || scoreDataList.scores == null)
        {
            Debug.LogError("점수 데이터가 null입니다. JSON 응답: " + responseData.response);
            return; // 오류 발생 시 메서드 종료
        }

        // 점수 목록 초기화
        string scoreList = "순위\t이름\t점수\n"; // 헤더 추가

        for (int i = 0; i < Mathf.Min(scoreDataList.scores.Length, 10); i++)
        {
            scoreList += $"{i + 1}. {scoreDataList.scores[i].name} {scoreDataList.scores[i].score}\n"; // 점수 추가
        }

        // 점수 표시 텍스트 업데이트
        scoreDisplayText.text = scoreList; // 모든 점수를 하나의 텍스트로 설정
        Debug.Log("Updated score display text: " + scoreList);
    }
}