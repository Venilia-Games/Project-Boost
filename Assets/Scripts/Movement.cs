using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float mainThrust = 1000f;
    [SerializeField] private float rotationThrust = 100f;
    [SerializeField] private AudioClip mainEngine;

    [SerializeField] private ParticleSystem mainBooster;
    [SerializeField] private ParticleSystem leftBooster;
    [SerializeField] private ParticleSystem rightBooster;
    
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
        
    }
    
    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotation();
        }
    }
    
    private void StartThrusting()
    {
        var scaledThrust = Vector3.up * (mainThrust * Time.deltaTime);
        _rigidbody.AddRelativeForce(scaledThrust);
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(mainEngine);
        }

        if (!mainBooster.isPlaying)
        {
            mainBooster.Play();
        }
    }

    private void StopThrusting()
    {
        _audioSource.Stop();
        mainBooster.Stop();
    }
    
    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightBooster.isPlaying)
        {
            rightBooster.Play();
        }
    }
    
    private void RotateRight()
    {
        ApplyRotation(-rotationThrust);
        if (!leftBooster.isPlaying)
        {
            leftBooster.Play();
        }
    }

    private void StopRotation()
    {
        rightBooster.Stop();
        leftBooster.Stop();
    }


    

    private void ApplyRotation(float rotationThisFrame)
    {
        //freezing rotation to manually rotate
        _rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * (Time.deltaTime * rotationThisFrame));
        _rigidbody.freezeRotation = false;
    }
}
