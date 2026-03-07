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
    private AudioSource thrust_SFX;

    [SerializeField]
    private AudioSource crash_SFX;

    [SerializeField]
    private AudioSource success_SFX;

    [SerializeField]
    private ParticleSystem thrustParticles;

    [SerializeField]
    private ParticleSystem crashParticles;

    [SerializeField]
    private ParticleSystem rightThrustParticles;

    [SerializeField]
    private ParticleSystem leftThrustParticles;
    /* Components */

    private float thrustPower;
    private float steerPower;

    [HideInInspector]
    public bool isOnFloor;

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
            EmmitLeftThrustParticles(true);
        } 
        else if(steerInput.x < 0)
        {
            EmmitRightThrustParticles(true);
        } else
        {
            EmmitLeftThrustParticles(false);
            EmmitRightThrustParticles(false);
        }
    }

    void ProcessThrusting(float deltaTime)
    {
        if (thrustAction.IsPressed())
        {
            rigidbody.AddRelativeForce(thrustPower * deltaTime * Vector3.up);
            PlayThrustSFX(true);
            EmmitThrustParticles(true);
        } 
        else 
        {
            PlayThrustSFX(false);
            EmmitThrustParticles(false);
        }
    }

    void OnDisable()
    {
        steerAction.Disable();
        thrustAction.Disable();
    }

    public
    void PlayThrustSFX(bool play)
    {
        if(!play) thrust_SFX.Stop();
        else if(!thrust_SFX.isPlaying) thrust_SFX.Play();
    }

    public
    void PlayCrashSFX(bool play)
    {
        if(!play) crash_SFX.Stop();
        else if(!crash_SFX.isPlaying) crash_SFX.Play();
    }

    public
    void PlaySuccessSFX(bool play)
    {
        if(!play) success_SFX.Stop();
        else if(!success_SFX.isPlaying) success_SFX.Play();
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
    void EmmitThrustParticles(bool emmit)
    { 
        if(emmit) thrustParticles.Play();
        else thrustParticles.Stop();
    }

    public 
    void EmmitCrashParticles(bool emmit)
    {
        if(emmit) crashParticles.Play();
        else crashParticles.Stop();
    }

    public
    void EmmitRightThrustParticles(bool emmit)
    {
        if(emmit) rightThrustParticles.Play();
        else rightThrustParticles.Stop();
    }

    public
    void EmmitLeftThrustParticles(bool emmit)
    {
        if(emmit) leftThrustParticles.Play();
        else leftThrustParticles.Stop();
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
