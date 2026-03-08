using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /* Actions */
    private InputActionMap playerActions;
    private InputAction steerAction;
    private InputAction thrustAction;
    /* Actions */

    /* Components */
    #pragma warning disable
    private Rigidbody rigidbody;
    #pragma warning restore
    private AudioSource[] audioSources;
    private ParticleSystem[] particleSystems;

    [SerializeField]
    private SFX thrustSFX;

    public SFX crashSFX;
    public SFX successSFX;

    [SerializeField]
    private ParticleSystem thrustParticles;

    public ParticleSystem crashParticles;

    [SerializeField]
    private ParticleSystem rightThrustParticles;

    [SerializeField]
    private ParticleSystem leftThrustParticles;
    /* Components */

    private float thrustPower;
    private float steerPower;

    [HideInInspector]
    public bool isOnFloor;

    [HideInInspector]
    public bool crashed;

    [HideInInspector]
    public bool finished;

    void OnEnable()
    {
        steerAction?.Enable();
        thrustAction?.Enable();
    }

    void Awake()
    {
        GameManager.Player = this;
    }

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        rigidbody = GetComponent<Rigidbody>();

        playerActions = InputSystem.actions.FindActionMap("Player");
        steerAction = playerActions.FindAction("Move");
        thrustAction = playerActions.FindAction("Jump");
        
        steerAction.Enable();
        thrustAction.Enable();
        // playerActions.Enable(); // alternatively; although not needed

        thrustPower = 100.0f * Physics.gravity.magnitude * rigidbody.mass;
        steerPower = 100.0f;

        isOnFloor = true;
        crashed = false;
        finished = false;
    }

    void FixedUpdate()
    {
        ProcessThrusting(Time.fixedDeltaTime);
        ProcessSteering(Time.fixedDeltaTime);
    }

    void ProcessSteering(float deltaTime)
    {
        if(isOnFloor) return;
        rigidbody.freezeRotation = true; // Reminds me of mutex locks :)
        Vector2 steerInput = steerAction.ReadValue<Vector2>();
        Vector3 steeringAxis = Vector3.Cross(Vector3.up, steerInput).normalized;
        transform.Rotate(steerPower * deltaTime * steeringAxis); // Crossing prevents rotating on X.
        rigidbody.freezeRotation = false;

        if(steerInput.x > 0)
        {
            leftThrustParticles.Play();
        } 
        else if(steerInput.x < 0)
        {
            rightThrustParticles.Play();
        } 
        else
        {
            rightThrustParticles.Stop();
            leftThrustParticles.Stop();
        }
    }

    void ProcessThrusting(float deltaTime)
    {
        if (thrustAction.IsPressed())
        {
            rigidbody.AddRelativeForce(thrustPower * deltaTime * Vector3.up);
            thrustSFX.Play();
            thrustParticles.Play();
        } 
        else 
        {
            thrustSFX.Stop();
            thrustParticles.Stop();
        }
    }

    void OnDisable()
    {
        steerAction.Disable();
        thrustAction.Disable();
    }

    public
    void StopAllAudio()
    {
        foreach(AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
        }
    }

    public
    void StopAllParticles()
    {
        foreach(ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Stop();
        }
    }
}
