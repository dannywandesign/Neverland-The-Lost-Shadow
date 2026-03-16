using UnityEngine;
using TMPro; // Important for TextMeshPro

public class GlobalShop : MonoBehaviour
{
    [Header("UI References")]
    public GameObject shopPanel;
    public TextMeshProUGUI shardText; // Drag your ShardCounterText here

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }
    }

    public void ToggleShop()
    {
        bool isOpening = !shopPanel.activeSelf;
        shopPanel.SetActive(isOpening);

        if (isOpening)
        {
            Time.timeScale = 0f; 
            shardText.color = Color.white; // Turn white in shop
            
            Cursor.visible = true; 
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f; 
            shardText.color = Color.black; // Turn back to black
            
            Cursor.visible = false; 
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}