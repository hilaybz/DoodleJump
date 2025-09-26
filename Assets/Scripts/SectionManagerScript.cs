using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    [Header("Prefab / Parenting")]
    [SerializeField] private Section sectionPrefab; // the Section prefab to spawn
    [SerializeField] private Transform container;   // optional parent for spawned Sections (can be left empty)

    [Header("Layout")]
    [SerializeField] private float sectionEvery = 6f; // how much to increase Y for each next Section
    [SerializeField] private int sectionsAmount = 12; // how many Sections to create at Start

    [Header("Normal Platform Rule")]
    [Tooltip("Every Nth platform will be a guaranteed NORMAL (uses first prefab in the Section's table). 0 = never force.")]
    [SerializeField] private int normalPlatformEvery = 4;

    // internal: where to place the next recycled/new Section on Y
    private float nextY;

    // internal: keep references to live Sections (not strictly required, but handy)
    private readonly List<Section> sections = new List<Section>();

    // internal: count how many platforms since we last spawned a NORMAL
    // when this reaches (normalPlatformEvery - 1), the next spawn will be forced normal
    private int platformsSinceLastNormal = 0;

    private void Start()
    {
        // start spawning at this object's Y
        nextY = transform.position.y;

        // create initial batch of Sections stacked upward
        for (int i = 0; i < sectionsAmount; i++)
        {
            // decide if this Section should force a normal platform
            bool forceNormal = ShouldForceNormal();

            // spawn one Section at nextY
            Section s = Instantiate(sectionPrefab,
                                    new Vector3(transform.position.x, nextY, 0f),
                                    Quaternion.identity,
                                    container ? container : transform);

            // initialize the Section (forceNormal + give it this manager)
            s.Init(forceNormal, this);

            // keep a reference
            sections.Add(s);

            // step Y for the next Section
            nextY += sectionEvery;
        }
    }

    // called by a Section after it decides/spawns (or not) a platform
    // true  = spawned a NORMAL platform (first prefab in table)
    // false = spawned a non-normal OR nothing
    public void ReportPlatformSpawned(bool isNormal)
    {
        if (normalPlatformEvery <= 0)
            return; // no forcing logic desired

        if (isNormal)
        {
            // reset the counter whenever a normal platform actually appears
            platformsSinceLastNormal = 0;
        }
        else
        {
            // increase until we reach the threshold; next spawn will be forced
            platformsSinceLastNormal++;
        }
    }

    // called by a Section when it goes below the screen and wants to be reused
    public void UpdateSection(Section s)
    {
        if (s == null) return;

        // move the section up
        s.transform.position = new Vector3(transform.position.x, nextY, 0f);

        // decide if this recycled section should force a normal platform
        bool forceNormal = ShouldForceNormal();

        // just spawn a new platform, don’t re-init everything
        s.SpawnPlatform(forceNormal);

        // advance Y
        nextY += sectionEvery;
    }


    // helper: decide whether the next Section should FORCE a normal platform
    private bool ShouldForceNormal()
    {
        // if 0 or negative, never force
        if (normalPlatformEvery <= 0) return false;

        // when we have placed (normalPlatformEvery - 1) non-normal platforms,
        // the next one should be forced normal to keep the rhythm
        return platformsSinceLastNormal >= (normalPlatformEvery - 1);
    }
}
