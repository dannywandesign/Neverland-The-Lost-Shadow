using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // Required for Coroutines

public class LevelExit : MonoBehaviour
{
    [Header("Settings")]
    public int worldIndexToUnlock = 1; 
    public string menuSceneName = "Worlds";

    [Header("Notification Settings")]
    public TextMeshProUGUI notificationText;
    public Color lockedTextColor = Color.yellow;
    public float displayDuration = 3f;

    private bool isDisplaying = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ShardManager.instance != null)
            {
                int current = ShardManager.instance.shardCount;
                int total = ShardManager.instance.totalShardsInLevel;

                if (current < total)
                {
                    // If we aren't already showing the message, start the timer
                    if (!isDisplaying)
                    {
                        StartCoroutine(ShowLockedMessage(total - current));
                    }
                    return; 
                }
            }

            Cursor.visible = true; // Show the mouse
            Cursor.lockState = CursorLockMode.None; // Unlock the mouse

            // Success: Unlock and leave
            GameSession.UnlockWorld(worldIndexToUnlock);
            SceneManager.LoadScene(menuSceneName);
        }
    }

    IEnumerator ShowLockedMessage(int shardsLeft)
    {
        isDisplaying = true;

        if (notificationText != null)
        {
            // Set your custom color and text
            notificationText.color = lockedTextColor;
            notificationText.text = "LEVEL LOCKED\nNeed " + shardsLeft + " more shards!";
            
            // Turn on the canvas (assumes text is inside the PopupCanvas)
            notificationText.transform.parent.gameObject.SetActive(true);

            // Wait for the time you set in the Inspector
            yield return new WaitForSeconds(displayDuration);

            // Hide it
            notificationText.transform.parent.gameObject.SetActive(false);
        }

        isDisplaying = false;
    }
}