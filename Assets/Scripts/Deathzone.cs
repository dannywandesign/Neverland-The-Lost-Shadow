using UnityEngine;
using UnityEngine.SceneManagement;

public class Deathzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Reset the shards in the manager
            if (ShardManager.instance != null)
            {
                ShardManager.instance.ResetShards();
            }

            // 2. Reload the scene (This is the best way to make sure 
            // all physical shards respawn on the map too!)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
            Debug.Log("Peter fell! Shards lost, returning to start.");
        }
    }
}