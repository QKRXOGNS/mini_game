using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void GameOver()
    {
        Debug.Log("Restarting game...");
        Invoke(nameof(RestartGame), 1.5f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
