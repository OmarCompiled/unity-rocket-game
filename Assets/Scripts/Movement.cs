using System.Linq;
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
    /* Components */

    private float thrust;

    void OnEnable()
    {
        
        steerAction?.Enable();
        thrustAction?.Enable();
    }

    void Start()
    {   
        Physics.gravity = new Vector3(0, -4, 0);
        rigidbody = GetComponent<Rigidbody>();

        playerActions = InputSystem.actions.FindActionMap("Player");
        steerAction = playerActions.FindAction("Move");
        thrustAction = playerActions.FindAction("Jump");
        
        steerAction.Enable();
        thrustAction.Enable();
        // playerActions.Enable(); // alternatively; although not needed

        thrust = 100.0f * Physics.gravity.magnitude * rigidbody.mass;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        ProcessThrust(Time.fixedDeltaTime);
        ProcessSteering();
    }

    void ProcessSteering()
    {
        if (steerAction.IsPressed())
        {
            Vector2 steer = steerAction.ReadValue<Vector2>();
            
            rigidbody.AddTorque(Vector3.Cross(Vector3.up, steer) * 0.5f); // Crossing prevents rotating on X.
        }
    }

    void ProcessThrust(float deltaTime)
    {
        if (thrustAction.IsPressed())
        {
            rigidbody.AddRelativeForce(thrust * deltaTime * Vector3.up);
        }
    }

    void OnDisable()
    {
        steerAction.Disable();
        thrustAction.Disable();
    }
}
