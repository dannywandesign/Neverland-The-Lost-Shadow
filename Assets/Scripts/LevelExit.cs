using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string unlockWorldKey; // Set this to "World1_Unlocked" in the Tutorial
    public string menuSceneName = "Worlds";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Unlock the next world!
            PlayerPrefs.SetInt(unlockWorldKey, 1);
            PlayerPrefs.Save(); // Force-save to the computer

            // Go back to the World selection screen
            SceneManager.LoadScene(menuSceneName);
        }
    }
}