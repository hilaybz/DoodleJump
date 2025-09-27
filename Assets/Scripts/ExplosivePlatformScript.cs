using UnityEngine;

public class ExplosivePlatformScript : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosiveSFX;
    [SerializeField, Range(0f, 1f)] private float volume = 0.8f;

    [Header("Animation")]
    [SerializeField] private Animator animator; // drag your Animator here
    [SerializeField] private string explodeTrigger = "Explode"; // Animator trigger name

    [Header("Sorting")]
    [SerializeField] private string platformLayer = "Platforms"; // idle layer
    [SerializeField] private int platformOrder = 0;
    [SerializeField] private string explosionLayer = "FX";       // boom layer (above Player)
    [SerializeField] private int explosionOrder = 0;

    private bool exploded;
    private SpriteRenderer sr;

    private void Awake()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;   // don’t play automatically
        audioSource.spatialBlend = 0f;     // 2D sound

        if (!animator) animator = GetComponent<Animator>(); // auto-assign if Animator is on same GameObject
        sr = GetComponent<SpriteRenderer>();


        // ensure idle starts behind the player
        if (sr)
        {
            sr.sortingLayerName = platformLayer;
            sr.sortingOrder = platformOrder;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (exploded) return; // one-shot

        if (collision.gameObject.CompareTag("Player") && collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                exploded = true;

                var col = GetComponent<Collider2D>();
                if (col) col.enabled = false; // let player fall through

                // promote to FX so explosion draws over the player
                if (sr)
                {
                    sr.sortingLayerName = explosionLayer;
                    sr.sortingOrder = explosionOrder;
                }

                // Play animation
                if (animator)
                    animator.SetTrigger(explodeTrigger);

                // Play the sound
                if (audioSource && explosiveSFX)
                    audioSource.PlayOneShot(explosiveSFX, volume);

                // TODO: Handle game over logic here (disable player, show UI, etc.)
            }
        }
    }


    // called by Animation Event at the end of Explosive_Explosion clip
    public void OnExplosionFinished()
    {
        Destroy(gameObject);
    }
}
