using UnityEngine;

public class DeadDoodleScript : MonoBehaviour
{
    public GameManagerScript gameManager;
    private Camera mainCam;
    [SerializeField] private float offset = -0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float camBottom = mainCam.transform.position.y - mainCam.orthographicSize;
        // add a small buffer so it's slightly below the screen
        float deathY = camBottom - offset;

        // check if player has fallen below screen
        if (transform.position.y < deathY)
        {
            Debug.Log("Player has fallen below the screen. Game Over!");
            gameManager.gameOver();
            
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic; // disables gravity & collisions
            }
        }
    }
}
