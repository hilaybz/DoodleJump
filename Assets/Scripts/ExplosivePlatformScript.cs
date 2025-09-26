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

    private void Awake()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;   // don’t play automatically
        audioSource.spatialBlend = 0f;     // 2D sound

        if (!animator) animator = GetComponent<Animator>(); // auto-assign if Animator is on same GameObject
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
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
}
