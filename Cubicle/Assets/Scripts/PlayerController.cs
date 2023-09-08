using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //private variables
    PlayerInputs inputActions;
    Rigidbody playerRigidBody;
    bool grounded;
    
    //Serialized Private Variables
    [SerializeField] float playerSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] Transform playerCamera;


    private void Awake()
    {
        inputActions = new PlayerInputs();
        playerRigidBody = GetComponentInParent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    /// <summary>
    /// Calculates the correct move direction for the player relative to the camera
    /// </summary>
    /// <returns></returns>
    Vector3 CalculateMoveDirection()
    {
        Vector3 moveDirection = inputActions.PlayerMovement.BasicMovement.ReadValue<Vector2>();
        moveDirection = Vector3.ProjectOnPlane((playerCamera.right * moveDirection.x) + (playerCamera.forward * moveDirection.y), Vector3.up);
        return moveDirection.normalized;
    }

    /// <summary>
    /// Checks if the Player's Speed is within it's limits and determins if force can be added
    /// </summary>
    private void ApplyMovement()
    {
        //If the player has reached the max speed just return this method
        if (playerRigidBody.velocity.magnitude > maxSpeed)
        {
            return;
        }
        Debug.Log("Current speed: " + playerRigidBody.velocity.magnitude);

        playerRigidBody.AddForce(CalculateMoveDirection() * playerSpeed, ForceMode.VelocityChange);
    }

    //Action Methods

    public void Jump(InputAction.CallbackContext context)
    {
        playerRigidBody.AddForce(jumpHeight * transform.up, ForceMode.Impulse);
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.PlayerMovement.Jump.performed += Jump;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.PlayerMovement.Jump.performed -= Jump;
    }

    
}
