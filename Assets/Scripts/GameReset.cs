using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReset : MonoBehaviour
{
    // This part handles the "R" key while playing
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
    }

    public void ResetLevel()
    {
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // --- THE MAGIC PART ---
    // This attribute tells Unity: "Run this ONCE when the app opens, and NEVER AGAIN."
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ColdBootReset()
    {
        // List your IDs here just like in the other script
        string[] idsToWipe = { "0", "1", "2", "3" };

        foreach (string id in idsToWipe)
        {
            PlayerPrefs.DeleteKey(id);
        }
        
        PlayerPrefs.DeleteKey("World4_Unlocked");
        PlayerPrefs.Save();
        
        Debug.Log("Game Booted: Memory Wiped. This will not happen again until you restart the app.");
    }
}