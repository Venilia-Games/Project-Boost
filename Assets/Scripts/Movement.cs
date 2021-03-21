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
            var scaledThrust = Vector3.up * (mainThrust * Time.deltaTime);
            _rigidbody.AddRelativeForce(scaledThrust);
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.Stop();
        }
        
    }

    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationThrust);
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        //freezing rotation to manually rotate
        _rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * (Time.deltaTime * rotationThisFrame));
        _rigidbody.freezeRotation = false;
    }
}
