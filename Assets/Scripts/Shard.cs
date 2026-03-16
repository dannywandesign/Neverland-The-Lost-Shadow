using UnityEngine;

public class Shard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object touching the shard is the Player
        if (other.CompareTag("Player"))
        {
            // Tell the manager to add a shard
            ShardManager.instance.AddShard();

            // Destroy the shard so it can't be collected again
            Destroy(gameObject);
        }
    }
}