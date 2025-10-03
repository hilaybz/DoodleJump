using UnityEngine;

public class DeadDoodleScript : MonoBehaviour
{
    public GameManagerScript gameManager;
    private Camera mainCam;

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
        float deathY = camBottom - 0.5f;

        // check if player has fallen below screen
        if (transform.position.y < deathY)
        {
            gameManager.gameOver();
        }
    }
}
