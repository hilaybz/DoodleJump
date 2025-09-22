using UnityEngine;

public class DoodleFaceFlipScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        if (!rb) return;

        if (rb.linearVelocity.x > 0.05f)
            transform.localScale = new Vector3(-1, 1, 1);   // facing right
        else if (rb.linearVelocity.x < -0.05f)
            transform.localScale = new Vector3(1, 1, 1);  // facing left
    }
}
