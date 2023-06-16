using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceBehavior : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Lobby");
    }
}
