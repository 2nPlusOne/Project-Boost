using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float thrustForce = 1000f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float thrustVolume = 0.5f;
    [SerializeField] float volumeFadeSpeed = 0.1f;
    
    AudioSource thrustAudio;
    
    Rigidbody rb;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        thrustAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);

            thrustAudio.volume = thrustVolume;
            if (!thrustAudio.isPlaying)
                thrustAudio.Play();
        }
        else if (thrustAudio.isPlaying && thrustAudio.volume > 0)
            AudioFade(thrustAudio, volumeFadeSpeed);
    }

    private void AudioFade(AudioSource source, float fadeSpeed)
    {
        if (source.volume > 0)
            source.volume = source.volume - fadeSpeed;
        else 
            source.Stop();
    }

    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationSpeed);
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // Freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // Unfreezing rotation so the physics system can take over
    }
}
