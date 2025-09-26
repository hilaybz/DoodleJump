using UnityEngine;

public class Section : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private SectionManager manager; // set by SectionManager when creating this Section

    [Header("Spawn Control")]
    [SerializeField, Range(0f, 1f)] private float spawnChance = 0.85f; // chance this section spawns anything (when not forced)
    [Tooltip("Inset from screen edges so platforms don't clip offscreen (world units).")]
    [SerializeField] private float horizontalPadding = 0.3f;

    [Header("Camera / Cleanup")]
    [SerializeField] private Camera targetCamera; // camera reference
    [SerializeField] private float destroyOffset = 2f; // how far below screen until this section recycles

    // ---------- Weighted table ----------
    [System.Serializable]
    public struct WeightedPrefab
    {
        public GameObject prefab;      // assign a prefab
        [Min(0f)] public float weight; // relative chance
    }

    [Header("Weighted Spawn Table")]
    [Tooltip("Add entries like: Normal (0.9), Broken (0.1). Weights are relative. FIRST ONE SHOULD BE NORMAL")]
    [SerializeField] private WeightedPrefab[] spawnTable;
    // -----------------------------------

    private float xRangeFromCamera;   // computed half-width minus padding
    private GameObject currentPlatform; // only one platform per section

    // called by SectionManager when creating this Section
    public void Init(bool forceNormal, SectionManager m)
    {
        manager = m;

        // Camera fallback
        if (!targetCamera) targetCamera = Camera.main;
        if (!targetCamera || !targetCamera.orthographic) return;

        // Compute horizontal spawn range from camera
        float camH = 2f * targetCamera.orthographicSize;
        float camW = camH * targetCamera.aspect;
        xRangeFromCamera = Mathf.Max(0f, camW * 0.5f - horizontalPadding);

        // spawn immediately when initialized
        SpawnPlatform(forceNormal);
    }

    private void Update()
    {
        if (!targetCamera) return;

        // bottom of the camera
        float camBottomY = targetCamera.transform.position.y - targetCamera.orthographicSize;

        // when this Section goes below the screen
        if (transform.position.y + destroyOffset < camBottomY)
        {
            ClearPlatform(); // destroy old platform
            if (manager != null)
                manager.UpdateSection(this); // tell manager to recycle this Section
            else
                Destroy(gameObject); // fallback if no manager
        }
    }

    // Spawns a platform in this section
    public void SpawnPlatform(bool forceNormal)
    {
        ClearPlatform(); // ensure only one platform exists

        GameObject toSpawn = null;
        bool spawnedNormal = false;

        if (forceNormal && spawnTable.Length > 0 && spawnTable[0].prefab != null)
        {
            // first entry in table = guaranteed normal platform
            toSpawn = spawnTable[0].prefab;
            spawnedNormal = true;
        }
        else
        {
            // random spawn based on chance and weighted table
            if (Random.value <= spawnChance)
            {
                toSpawn = PickWeightedPrefab();
                if (toSpawn != null && toSpawn == spawnTable[0].prefab)
                    spawnedNormal = true;
            }
        }

        if (toSpawn != null)
        {
            float y = transform.position.y;
            float x = Random.Range(-xRangeFromCamera, xRangeFromCamera);

            currentPlatform = Instantiate(toSpawn, new Vector3(x, y, 0f), Quaternion.identity);
            currentPlatform.transform.SetParent(transform, true);
        }

        // report back to manager if platform was normal or not
        if (manager != null)
            manager.ReportPlatformSpawned(spawnedNormal);
    }

    // Picks a prefab by weight
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

    // Destroys the currently spawned platform (if any)
    private void ClearPlatform()
    {
        if (currentPlatform != null)
        {
            Destroy(currentPlatform);
            currentPlatform = null;
        }
    }
}
