using UnityEngine;

public class DestroyOffScreenScript : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float offset = 1f; // how far below the screen before destroying

    private void Start()
    {
        // default to main camera if none set
        if (!targetCamera) targetCamera = Camera.main;
    }

    private void Update()
    {
        if (!targetCamera) return;

        // bottom edge of camera
        float camBottom = targetCamera.transform.position.y - targetCamera.orthographicSize;

        // if this platform is lower than bottom - offset so destroy it
        if (transform.position.y < camBottom - offset)
        {
            Destroy(gameObject);
        }
    }
}
