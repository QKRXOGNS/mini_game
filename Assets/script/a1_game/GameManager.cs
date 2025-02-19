using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 관련 네임스페이스 추가

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private float score;
    private float survivalTime;
    private string scoreFilePath;
    private float maxScore; // 최대 점수 변수 추가

    public Text scoreText; // 점수를 표시할 UI Text 오브젝트
    public Text maxScoreText; // 최대 점수를 표시할 UI Text 오브젝트
    public Button restartButton; // 재시작 버튼

    private void Awake()
    {
        if (instance == null) instance = this;
        scoreFilePath = Path.Combine(Application.persistentDataPath, "score.json");
        LoadScore();
    }

    private void Start()
    {
        score = 0; // 점수 초기화
        survivalTime = 0; // 생존 시간 초기화
        StartCoroutine(ScoreIncrement());
        UpdateScoreText(); // 초기 점수 텍스트 업데이트
        maxScoreText.text = "Max Score: " + maxScore; // 최대 점수 초기화
        restartButton.gameObject.SetActive(false); // 재시작 버튼 숨기기
    }

    private IEnumerator ScoreIncrement()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초마다 점수 증가
            survivalTime += 1f;
            score += 10f; // 생존 시간에 따라 점수 증가
            UpdateScoreText(); // 점수 업데이트
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Saving score...");
        Debug.Log("Final Score: " + score); // 최종 점수 로그 출력
        Debug.Log("Survival Time: " + survivalTime); // 생존 시간 로그 출력

        // 현재 점수가 최대 점수보다 클 경우에만 저장
        if (score > maxScore)
        {
            maxScore = score; // 최대 점수 업데이트
            SaveScore(); // 게임 오버 시 점수 저장
        }

        // UI 업데이트
        scoreText.gameObject.SetActive(false); // 점수 텍스트 숨기기
        maxScoreText.text = "Max Score: " + maxScore; // 최대 점수 업데이트
        maxScoreText.gameObject.SetActive(true); // 최대 점수 텍스트 보이기
        restartButton.gameObject.SetActive(true); // 재시작 버튼 보이기
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SaveScore()
    {
        ScoreData scoreData = new ScoreData
        {
            Score = maxScore, // 최대 점수 저장
            SurvivalTime = survivalTime
        };
        string json = JsonUtility.ToJson(scoreData);
        File.WriteAllText(scoreFilePath, json);
        
        Debug.Log("Score saved to: " + scoreFilePath); // 저장 경로 로그 출력
    }

    private void LoadScore()
    {
        if (File.Exists(scoreFilePath))
        {
            string json = File.ReadAllText(scoreFilePath);
            var scoreData = JsonUtility.FromJson<ScoreData>(json);
            maxScore = scoreData.Score; // 최대 점수 불러오기
            survivalTime = scoreData.SurvivalTime;
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score; // 점수 텍스트 업데이트
    }

    [System.Serializable]
    private class ScoreData
    {
        public float Score;
        public float SurvivalTime;
    }
}
