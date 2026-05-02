using UnityEngine;

public class ScrollCollectible : MonoBehaviour
{
    [Header("Unique ID")]
    // This MUST match the IDs in your World4Gatekeeper list (0, 1, 2, or 3)
    public string UniqueID;

    void Start()
    {
        // Check if the memory says this scroll was already picked up
        if (PlayerPrefs.GetInt(UniqueID, 0) == 1)
        {
            // If it was already found, remove it immediately
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object touching the scroll is tagged "Player"
        if (other.CompareTag("Player"))
        {
            // Save to the game's memory that this ID is collected (1 = true)
            PlayerPrefs.SetInt(UniqueID, 1);
            PlayerPrefs.Save();

            Debug.Log("Collected Scroll: " + UniqueID);

            // Destroy the scroll so it's gone from the level
            Destroy(gameObject);
        }
    }
}