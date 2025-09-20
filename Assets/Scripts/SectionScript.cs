using UnityEngine;

public class Section : MonoBehaviour
{
    [Header("Spawn Control")]
    [SerializeField, Range(0f, 1f)] private float spawnChance = 0.85f; // chance this section spawns anything
    [Tooltip("Inset from screen edges so platforms don't clip offscreen (world units).")]
    [SerializeField] private float horizontalPadding = 0.3f;

    [Header("Camera / Cleanup")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float destroyOffset = 2f;

    // ---------- Weighted table ----------
    [System.Serializable]
    public struct WeightedPrefab
    {
        public GameObject prefab;     // assign a prefab
        [Min(0f)] public float weight; // relative chance (e.g., 0.9, 0.1)
    }

    [Header("Weighted Spawn Table")]
    [Tooltip("Add entries like: Normal (0.9), Broken (0.1). Weights are relative.")]
    [SerializeField] private WeightedPrefab[] spawnTable;
    // -----------------------------------

    private float xRangeFromCamera; // computed half-width minus padding

    private void Start()
    {
        // Camera fallback
        if (!targetCamera) targetCamera = Camera.main;
        if (!targetCamera || !targetCamera.orthographic) return;

        // Compute horizontal spawn range from camera
        float camH = 2f * targetCamera.orthographicSize;
        float camW = camH * targetCamera.aspect;
        xRangeFromCamera = Mathf.Max(0f, camW * 0.5f - horizontalPadding);

        // Roll the "do we spawn at all" check
        if (Random.value > spawnChance) return;

        // Pick a prefab by weight
        GameObject prefab = PickWeightedPrefab();
        if (!prefab) return;

        // Place it at this section's Y, random X within camera width (with padding)
        float y = transform.position.y;
        float x = Random.Range(-xRangeFromCamera, xRangeFromCamera);

        var go = Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity);
        go.transform.SetParent(transform, true); // cleans up with this Section
    }

    private void Update()
    {
        if (!targetCamera) return;
        float camBottomY = targetCamera.transform.position.y - targetCamera.orthographicSize;

        if (transform.position.y + destroyOffset < camBottomY)
            Destroy(gameObject);
    }

    // ---- Weighted pick: sum weights, roll, pick the bucket ----
    private GameObject PickWeightedPrefab()
    {
        if (spawnTable == null || spawnTable.Length == 0) return null;

        float total = 0f;
        for (int i = 0; i < spawnTable.Length; i++)
            total += Mathf.Max(0f, spawnTable[i].weight);

        if (total <= 0f) return null;

        float r = Random.value * total;
        float accum = 0f;

        for (int i = 0; i < spawnTable.Length; i++)
        {
            float w = Mathf.Max(0f, spawnTable[i].weight);
            accum += w;
            if (r <= accum)
                return spawnTable[i].prefab;
        }
        return spawnTable[spawnTable.Length - 1].prefab; // fallback
    }
}
