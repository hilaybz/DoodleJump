using UnityEngine;

public class Section : MonoBehaviour
{
    [Header("Platforms")]
    [SerializeField] private GameObject[] platformPrefabs;     // all possible platform prefabs
    [SerializeField, Range(0f, 1f)] private float spawnChance = 0.85f; // chance to spawn one
    [Tooltip("Extra inset from the screen edges so platforms don't clip offscreen.")]
    [SerializeField] private float horizontalPadding = 0.3f;   // world units to keep away from edges

    [Header("Cleanup")]
    [SerializeField] private Camera targetCamera;               // camera to compare/dimension from
    [SerializeField] private float destroyOffset = 2f;          // how far below camera before destroy

    // computed at runtime from the camera (half of visible width minus padding)
    private float xRangeFromCamera;

    void Start()
    {
        // Pick camera
        if (targetCamera == null) targetCamera = Camera.main;
        if (!targetCamera || !targetCamera.orthographic) return;

        // === Compute xRange from the camera ===
        float camHeight = 2f * targetCamera.orthographicSize;     // world units
        float camWidth = camHeight * targetCamera.aspect;        // world units
        xRangeFromCamera = Mathf.Max(0f, camWidth * 0.5f - horizontalPadding);

        // Maybe spawn a single platform at this section's Y
        if (platformPrefabs.Length > 0 && Random.value <= spawnChance)
        {
            float y = transform.position.y;
            float x = Random.Range(-xRangeFromCamera, xRangeFromCamera);

            GameObject prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
            var go = Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity);
            go.transform.SetParent(transform, true); // so it gets cleaned up with the section
        }
    }

    void Update()
    {
        if (!targetCamera) return;

        // bottom of camera in world space
        float camBottomY = targetCamera.transform.position.y - targetCamera.orthographicSize;

        // destroy this Section (and its child platform) once it falls below view
        if (transform.position.y + destroyOffset < camBottomY)
        {
            Destroy(gameObject);
        }
    }
}
