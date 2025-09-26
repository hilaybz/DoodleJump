using UnityEngine;
using UnityEngine.UI;

public class CameraFollowScript : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // the doodle

    [Header("Score UI")]
    public Text scoreText;

    [Header("Camera Settings")]
    [Tooltip("How far above the player the camera should stay (world units).")]
    [SerializeField] private float verticalOffset = 2f; // adjustable in Inspector

    [Tooltip("How many points per unit of height.")]
    [SerializeField] private int scoreMultiplier = 10; // adjustable in Inspector

    private void LateUpdate()
    {
        // only move camera up if player has gone higher than current camera position - offset
        if (target.position.y > transform.position.y - verticalOffset)
        {
            float newY = target.position.y + verticalOffset;
            Vector3 newPosition = new Vector3(transform.position.x, newY, transform.position.z);
            transform.position = newPosition;
        }
    }

    private void Update()
    {
        // score = camera Y * multiplier
        scoreText.text = ((int)(transform.position.y * scoreMultiplier)).ToString();
    }
}
