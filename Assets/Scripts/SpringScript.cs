using UnityEngine;

public class SpringScript : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown = 0.05f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField, Range(0f, 1f)] private float volume = 0.8f;

    private float _lastJumpTime = -999f;



    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;   // don’t play automatically
        audioSource.spatialBlend = 0f;     // 2D sound
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision) { TryBounce(collision); }
    private void OnCollisionStay2D(Collision2D collision) {
        // TryBounce(collision);
    }


    private void TryBounce(Collision2D collision)
    {
        // Cooldown to avoid multi-trigger across frames
        if (Time.time - _lastJumpTime < jumpCooldown) return;

        if (collision.gameObject.CompareTag("Player") && collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 velocity = rb.linearVelocity;
                velocity.y = jumpForce;
                rb.linearVelocity = velocity;
                // Play the sound
                if (audioSource && jumpSFX)
                {
                    audioSource.PlayOneShot(jumpSFX, volume);
                }
                _lastJumpTime = Time.time;
            }
        }
    }
}
