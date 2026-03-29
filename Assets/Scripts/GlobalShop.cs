using UnityEngine;
using TMPro;

public class GlobalShop : MonoBehaviour
{
    [Header("UI References")]
    public GameObject shopPanel;
    public TextMeshProUGUI shardText; 

    void Start()
    {
        // AUTO-FIX: If Unity forgot the link, find them by name
        if (shopPanel == null)
        {
            shopPanel = GameObject.Find("ShopPanel");
        }

        if (shardText == null)
        {
            shardText = GameObject.Find("ShardCounterText")?.GetComponent<TextMeshProUGUI>();
        }

        // Ensure shop starts closed
        if (shopPanel != null) shopPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }
    }

    public void ToggleShop()
    {
        if (shopPanel == null)
        {
            Debug.LogError("GlobalShop: I can't find 'ShopPanel' in the Hierarchy! Check the name.");
            return;
        }

        bool isOpening = !shopPanel.activeSelf;
        shopPanel.SetActive(isOpening);

        if (isOpening)
        {
            Time.timeScale = 0f; 
            if (shardText != null) shardText.color = Color.white;
            Cursor.visible = true; 
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f; 
            if (shardText != null) shardText.color = Color.black;
            Cursor.visible = false; 
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}