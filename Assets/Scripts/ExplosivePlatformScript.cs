using UnityEngine;

public class ExplosivePlatformScript : MonoBehaviour
{

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosiveSFX;
    [SerializeField, Range(0f, 1f)] private float volume = 0.8f;


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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // FIX MAKE EXPLOSIVE ANIMATION
                // FIX MAKE GAME OVER
                // Play the sound
                if (audioSource && explosiveSFX)
                {
                    audioSource.PlayOneShot(explosiveSFX, volume);
                }
            }
        }
    }
}
