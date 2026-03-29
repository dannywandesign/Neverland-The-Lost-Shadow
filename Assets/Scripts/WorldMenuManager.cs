using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldMenuManager : MonoBehaviour
{
    [Header("Buttons in Order (Tutorial, W1, W2, W3, W4)")]
    public Button[] worldButtons; 

    void Start()
    {
        // 1. The Tutorial (Index 0) is ALWAYS unlocked for new players
        worldButtons[0].interactable = true;

        // 2. Loop through the rest of the buttons
        for (int i = 1; i < worldButtons.Length; i++)
        {
            // We create a key like "World1_Unlocked" or "World2_Unlocked"
            string key = "World" + i + "_Unlocked";
            
            // Check if that world is unlocked (1 = yes, 0 = no)
            bool isUnlocked = PlayerPrefs.GetInt(key, 0) == 1;

            // Set the button state
            worldButtons[i].interactable = isUnlocked;

            // Visual feedback: Make locked buttons semi-transparent
            Image img = worldButtons[i].GetComponent<Image>();
            if (img != null)
            {
                img.color = isUnlocked ? Color.white : new Color(1, 1, 1, 0.4f);
            }
        }
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Add this inside the WorldMenuManager class
    public void GoBackToStart()
    {
        // Make sure the name matches your StartScreen scene exactly!
        SceneManager.LoadScene("StartScreen");
    }
}