using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This variable stays true as long as the game is open.
    // It resets to false only when the app is fully closed and restarted.
    private static bool hasClearedProgressThisSession = false;

    void Awake()
    {
        if (!hasClearedProgressThisSession)
        {
            ResetAllProgress();
            hasClearedProgressThisSession = true;
            Debug.Log("New Session Started: All world progress and shards cleared.");
        }
    }

    public void GoToWorlds()
    {
        SceneManager.LoadScene("Worlds");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ResetAllProgress()
    {
        // Deletes everything: Unlocked worlds AND Banked Shards
        PlayerPrefs.DeleteAll();
        
        // If you only want to delete worlds but KEEP shards, use this instead:
        // PlayerPrefs.DeleteKey("World1_Unlocked");
        // PlayerPrefs.DeleteKey("World2_Unlocked");
        // ... etc
        
        PlayerPrefs.Save();
    }
}