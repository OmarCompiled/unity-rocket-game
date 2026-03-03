using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
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
    private Dictionary<string, AudioSource> audioSources;
    /* Components */

    private float thrustPower;
    private float steerPower;

    private bool isOnFloor;

    void OnEnable()
    {
        
        steerAction?.Enable();
        thrustAction?.Enable();
    }

    void Start()
    {   
        AudioSource[] audioSourcesArray = GetComponents<AudioSource>();
        audioSources = new Dictionary<string, AudioSource>();
        audioSources.Add("Thrust", audioSourcesArray[0]);
        rigidbody = GetComponent<Rigidbody>();

        Physics.gravity *= 0.5f;
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

    void Update()
    {

    }

    void FixedUpdate()
    {
        ProcessThrusting(Time.fixedDeltaTime);
        ProcessSteering(Time.fixedDeltaTime);
    }

    void ProcessSteering(float deltaTime)
    {
        if(isOnFloor) return;

        rigidbody.freezeRotation = true;

        Vector2 steerInput = steerAction.ReadValue<Vector2>();
        Vector3 steeringAxis = Vector3.Cross(Vector3.up, steerInput).normalized;
        transform.Rotate(steerPower * deltaTime * steeringAxis); // Crossing prevents rotating on X.

        rigidbody.freezeRotation = false;
    }

    void ProcessThrusting(float deltaTime)
    {
        if (thrustAction.IsPressed())
        {
            rigidbody.AddRelativeForce(thrustPower * deltaTime * Vector3.up);
            if(!audioSources["Thrust"].isPlaying) audioSources["Thrust"].Play();
        } 
        else 
        {
            audioSources["Thrust"].Stop();
        }
    }

    void OnDisable()
    {
        steerAction.Disable();
        thrustAction.Disable();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isOnFloor = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isOnFloor = false;
        }
    }
}
