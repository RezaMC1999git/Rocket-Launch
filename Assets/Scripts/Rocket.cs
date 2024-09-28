using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rtlThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip successSFX;
    [SerializeField] AudioClip deathSFX;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;
    enum State {Alive,Dying,trancending}
    State state = State.Alive;
    Rigidbody rigidBody;
    AudioSource audioSource;
    int currentLevel;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }
    void Rotate()
    {
        rigidBody.angularVelocity = Vector3.zero;
        float rotatinThisFrame = rtlThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward *  rotatinThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotatinThisFrame);
        }
    }
    void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if(state != State.Alive) { return; }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }
    void LoadNextLevel()
    {
        if (currentLevel == 0)
            SceneManager.LoadScene(1);
        if (currentLevel == 1)
            SceneManager.LoadScene(2);
        if (currentLevel == 2)
            SceneManager.LoadScene(0);
    }
    void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);
        mainEngineParticles.Play();
    }
    void StartSuccessSequence()
    {
        state = State.trancending;
        audioSource.Stop();
        audioSource.PlayOneShot(successSFX);
        Invoke("LoadNextLevel", 1f);
        successParticles.Play();
    }
    void StartDeathSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(deathSFX);
        state = State.Dying;
        deathParticles.Play();
        Invoke("LoadCurrnetLevelAgain", 1f);
    }
    void LoadCurrnetLevelAgain()
    {
        SceneManager.LoadScene(currentLevel);
    }
}
