using UnityEngine;

public class Shard : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug to see IF the collision is even happening
        Debug.Log("Shard: Something hit me! It was: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            if (ShardManager.instance != null)
            {
                ShardManager.instance.AddShard(value);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Shard: I can't find the ShardManager instance!");
            }
        }
    }
}