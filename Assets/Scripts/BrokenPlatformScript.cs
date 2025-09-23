using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BrokenPlatform : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float disableColliderDelay = 0.02f; // small delay so contact registers
    [SerializeField] private float destroyDelay = 0.5f;          // time to keep visuals after cracking
    [SerializeField, Range(0f, 1f)] private float volume = 0.8f;

    [Header("FX (optional)")]
    [SerializeField] private Animator animator;                  // has a "Crack" trigger
    [SerializeField] private AudioSource audioSource;            // crack SFX
    [SerializeField] private AudioClip crackSFX;                 // drag crack sound here


    private bool isBroken;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        if (!animator) animator = GetComponent<Animator>();
        if (!audioSource) audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()  // helpful if you ever pool this prefab
    {
        isBroken = false;
        if (col) col.enabled = true;
        if (animator) animator.ResetTrigger("Crack");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken) return;
        if (!collision.collider.CompareTag("Player")) return;

        // Only break when the player is landing ON the platform (coming down)
        if (collision.relativeVelocity.y > 0f) return;

        BreakNow();
    }

    private void BreakNow()
    {
        isBroken = true;

        if (animator) animator.SetTrigger("Crack");

        // play crack sound
        if (audioSource && crackSFX)
            audioSource.PlayOneShot(crackSFX, volume);

        // Let physics see the contact this frame, then drop the collider
        Invoke(nameof(DisableCollider), disableColliderDelay);

        // Remove the object after the animation/SFX
        Destroy(gameObject, destroyDelay);
    }

    private void DisableCollider()
    {
        if (col) col.enabled = false;
    }
}
