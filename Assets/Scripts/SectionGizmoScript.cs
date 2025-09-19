using UnityEngine;

public class SectionGizmo : MonoBehaviour
{
    [SerializeField] private float height = 6f;              // your section height
    [SerializeField] private Camera targetCamera;            // assign Main Camera
    [SerializeField] private Color fillColor = new(0f, 1f, 0f, 0.12f);
    [SerializeField] private Color lineColor = Color.green;

    private void OnDrawGizmos()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        if (targetCamera == null || !targetCamera.orthographic) return;

        // Camera world size
        float camWorldHeight = 2f * targetCamera.orthographicSize;
        float camWorldWidth = camWorldHeight * targetCamera.aspect;

        // Center on the camera X so it spans full width symmetrically
        float centerX = targetCamera.transform.position.x;
        Vector3 center = new(centerX, transform.position.y + height * 0.5f, 0f);

        // Draw
        Gizmos.color = fillColor;
        Gizmos.DrawCube(center, new Vector3(camWorldWidth, height, 0.01f));
        Gizmos.color = lineColor;
        Gizmos.DrawWireCube(center, new Vector3(camWorldWidth, height, 0.01f));
    }
}
