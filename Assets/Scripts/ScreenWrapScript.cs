using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    private float halfWidth;

    private void Start()
    {
        if (!targetCamera) targetCamera = Camera.main;

        // compute half camera width in world units
        float camHeight = 2f * targetCamera.orthographicSize;
        float camWidth = camHeight * targetCamera.aspect;
        halfWidth = camWidth / 2f;
    }

    private void LateUpdate()
    {
        if (!targetCamera) return;

        Vector3 pos = transform.position;

        // if player goes too far right, teleport to left
        if (pos.x > targetCamera.transform.position.x + halfWidth)
            pos.x = targetCamera.transform.position.x - halfWidth;

        // if player goes too far left, teleport to right
        else if (pos.x < targetCamera.transform.position.x - halfWidth)
            pos.x = targetCamera.transform.position.x + halfWidth;

        transform.position = pos;
    }
}
