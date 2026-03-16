using UnityEngine;

public class FaceForward : MonoBehaviour
{
    private Vector3 initialLocalScale;

    void Start()
    {
        // Store the tiny scale (0.005) you set in the Inspector
        initialLocalScale = transform.localScale;
    }

    void LateUpdate()
    {
        if (transform.parent == null) return;

        // 1. Lock Rotation so it never tilts
        transform.rotation = Quaternion.identity;

        // 2. Fix the Mirroring
        // If Peter's scale is -1, we set our local scale to -0.005.
        // -1 (Peter) * -0.005 (Text) = +0.005 (Global result is readable!)
        float parentDirection = Mathf.Sign(transform.parent.localScale.x);
        
        transform.localScale = new Vector3(
            initialLocalScale.x * parentDirection, 
            initialLocalScale.y, 
            initialLocalScale.z
        );
    }
}