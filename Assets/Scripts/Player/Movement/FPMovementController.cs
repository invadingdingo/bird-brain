using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPMovementController : MonoBehaviour {

    // Will be true when current perspective is active. 
    private bool isActiveController = false;
    
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    
    [HideInInspector]
    public bool canMove = true;

    private CameraController camController;
    public LayerMask groundLayer;

    void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start() {
        camController = playerCamera.GetComponent<CameraController>();
    
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update() {
        if (isActiveController) {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            
            // Press Left Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;

            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }
            
            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            if (!camController.overhead)
            {
                // Move the controller
                characterController.Move(moveDirection * Time.deltaTime);
            }
            else
            {
                characterController.Move(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * (Time.deltaTime * walkingSpeed));
            }
        
            // Player and Camera rotation
            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                if (!camController.overhead)
                {
                    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                    transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
                }    
            }
        }
    }

    public void SetActiveController(bool newState, Vector3 newPosition, string direction = "") {
        if (newState) {
            
            //Disable character controller to modify position. 
            characterController.enabled = false;
            
            // Disable collider for raycast otherwise ground will be detected at the top of player capsule..
            GetComponent<CapsuleCollider>().enabled = false;

            // Raycast to find the height of the ground being transitioned to.
            // Shoot ray down from 100 units up to check for intersection.
            Ray ray = new Ray(new Vector3(newPosition.x, 30f, newPosition.z), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 40f, groundLayer)) {
                // If intersection found, set newPosition.y to the point of intersection plus 1 to account for player height..
                newPosition.y = hit.point.y + 1;
            } else {
                Debug.Log("Ground not found!");
            }

            // Reenable collider after ground detection.
            GetComponent<CapsuleCollider>().enabled = true;

            // Depending on facing direction of 2D sprite, set FP controller to match. 
            switch (direction) {
                case "up":
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case "left":
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                case "right":
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case "down":
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                default:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
            }

            // Set new position.
            transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);

            // Reenable character controller. 
            characterController.enabled = true;
        }

        isActiveController = newState;
    }
}
