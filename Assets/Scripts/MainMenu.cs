using UnityEngine;
using UnityEngine.SceneManagement; // Essential for switching scenes

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Make sure the name inside the quotes matches your Scene file exactly!
        SceneManager.LoadScene("TutorialScreen");
    }

    public void QuitGame()
    {
        Debug.Log("Player Quit the Game");
        Application.Quit();
    }
}