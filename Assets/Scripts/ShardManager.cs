using UnityEngine;
using TMPro;

public class ShardManager : MonoBehaviour
{
    public static ShardManager instance;

    // Change 'int' to 'public int' so ShopLogic can see it
    public int shardCount; 
    public TextMeshProUGUI shardText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Reset shards on scene load as requested earlier
        shardCount = 0;
        UpdateUI();
    }

    public void AddShard()
    {
        shardCount++;
        UpdateUI();
    }

    // Add this new function so ShopLogic can subtract shards
    public void RemoveShards(int amount)
    {
        shardCount -= amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (shardText != null)
        {
            shardText.text = "SHARDS = " + shardCount;
        }
    }
}