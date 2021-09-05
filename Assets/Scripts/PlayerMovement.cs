using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float thrustForce = 1000f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float AudioVolume = 0.5f;
    [SerializeField] float volumeFadeSpeed = 0.1f;

    [SerializeField] AudioClip engineSound;
    
    [SerializeField] ParticleSystem mainThrusterParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    AudioSource audioSource;
    
    Rigidbody rb;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
            StartThrusting();

        else if (audioSource.isPlaying && audioSource.volume > 0)
            StopThrusting();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
            RotateLeft();

        else if (Input.GetKey(KeyCode.D))
            RotateRight();

        else
            StopRotating();
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);

        if (!mainThrusterParticles.isPlaying)
            mainThrusterParticles.Play();

        audioSource.volume = AudioVolume;
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(engineSound);
    }

    void StopThrusting()
    {
        AudioFade(audioSource, volumeFadeSpeed);
        mainThrusterParticles.Stop();
    }

    void AudioFade(AudioSource source, float fadeSpeed)
    {
        if (source.volume > 0)
            source.volume = source.volume - fadeSpeed;
        else
            source.Stop();
    }

    void RotateLeft()
    {
        ApplyRotation(rotationSpeed);

        if (!rightThrusterParticles.isPlaying)
            rightThrusterParticles.Play();
    }

    void RotateRight()
    {
        ApplyRotation(-rotationSpeed);

        if (!leftThrusterParticles.isPlaying)
            leftThrusterParticles.Play();
    }

    void StopRotating()
    {
        leftThrusterParticles.Stop();
        rightThrusterParticles.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // Freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // Unfreezing rotation so the physics system can take over
    }
}
