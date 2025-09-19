using UnityEngine;

public class Section : MonoBehaviour
{
    [Header("Platform Spawning")]
    [SerializeField] private GameObject platformPrefab;      // the platform to spawn
    [SerializeField, Range(0f, 1f)] private float spawnChance = 0.85f;
    [SerializeField] private float xRange = 2.4f;            // random X in [-xRange, +xRange]
    [SerializeField] private Transform parent;               // optional parent for hierarchy

    [Header("Cleanup")]
    [SerializeField] private Camera targetCamera;            // camera to compare against
    [SerializeField] private float destroyOffset = 2f;       // how far below the camera before destroy

    private void Start()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        if (platformPrefab && Random.value <= spawnChance)
        {
            float x = Random.Range(-xRange, xRange);
            float y = transform.position.y;
            var go = Instantiate(platformPrefab, new Vector3(x, y, 0f), Quaternion.identity);
            go.transform.SetParent(transform, true); // make platform a child of this Section
        }
    }

    private void Update()
    {
        if (targetCamera == null) return;

        // world Y position at the bottom of the camera view
        float camBottomY = targetCamera.transform.position.y - targetCamera.orthographicSize;

        // if section is far below, destroy
        if (transform.position.y + destroyOffset < camBottomY)
        {
            Destroy(gameObject);
        }
    }
}
