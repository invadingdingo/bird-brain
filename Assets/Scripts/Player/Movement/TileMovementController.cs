using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovementController : MonoBehaviour {

    [SerializeField] private float distanceToMove;
    [SerializeField] private float moveSpeed;

    // Sprite
    [SerializeField] private Sprite[] upWalk;
    [SerializeField] private Sprite[] downWalk;
    [SerializeField] private Sprite[] sideWalk;
    private SpriteRenderer sr;
    private float animationTimer = 0f; // Timer for animation.
    private float frameDuration = 0.1f; // Duration for each frame.
    private int currentFrame = 0; // Current frame index.

    // Will be true when current perspective is active. 
    private bool isActiveController = true;

    [SerializeField] public LayerMask collisions;

    // Directional rays to check each cardinal direction from player.
    private bool downCast;
    private bool upCast;
    private bool rightCast;
    private bool leftCast;

    Vector3 directionInput = Vector3.zero;
    private bool moveToPoint = false;
    private Vector3 endPosition;
    private bool moving;
    public string direction = "up";
    private Vector3 prevPos;

    private Vector3 facing;

    private bool canMove;
    private GameObject interactable;

    

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        endPosition = transform.position;
        moving = false;
        canMove = true;
        prevPos = transform.position;

    }

    void Update() {
    	if (canMove && isActiveController) {
    		HandleMovement();
            AnimationControl();
    	}
    }

    void FixedUpdate() {
            if (moveToPoint) {
                endPosition = new Vector3(endPosition.x, transform.position.y, endPosition.z);
                transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);

                if (transform.position != endPosition) {
                    
                    // Play footstep SFX.
                    if (!moving)
                        AudioManager.instance.PlayFootStep();

                    moving = true;
                } else {
                    moving = false;
                }
            }

            bool stillMoving = (transform.position != prevPos);

            prevPos = transform.position;
    }

    private void HandleMovement() {
    	downCast = Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(0, 0, -1), distanceToMove, collisions);
        upCast = Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(0, 0, 1), distanceToMove, collisions);
        leftCast = Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(-1, 0, 0), distanceToMove, collisions);
        rightCast = Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(1, 0, 0), distanceToMove, collisions);

        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(0, 0, -1) * distanceToMove, Color.red);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(0, 0, 1) * distanceToMove, Color.green);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(1, 0, 0) * distanceToMove, Color.blue);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(-1, 0, 0) * distanceToMove, Color.yellow);


        directionInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (directionInput != Vector3.zero && !moving) {
            if (directionInput.z == -1) {
                if (!downCast) { 
                endPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - distanceToMove); }
                direction = "down";
                facing = new Vector3(0, 0, -1);
            } else if (directionInput.x == -1) {
                if (!leftCast) { 
                endPosition = new Vector3(transform.position.x - distanceToMove, transform.position.y, transform.position.z); }
                direction = "left";
                facing = new Vector3(-1, 0, 0);
            } else if (directionInput.x == 1) {
                if (!rightCast) { 
                endPosition = new Vector3(transform.position.x + distanceToMove, transform.position.y, transform.position.z); }
                direction = "right";
                facing = new Vector3(1, 0, 0);
            } else if (directionInput.z == 1) {
                if (!upCast) { 
                endPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + distanceToMove); }
                direction = "up";
                facing = new Vector3(0, 0, 1);
            }
           	
            // Perform the raycast to check for a pushable object
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionInput, out hit, 0.7f)) {
                if (hit.collider.CompareTag("Pushable")) {
                    // Interact with the pushable object
                    hit.collider.gameObject.GetComponent<Rock>().Push(direction);
                    Debug.Log("Rock found.");
                } else {
                    Debug.Log("Space blocked by non-pushable object.");
                }
            } else {
                moveToPoint = true;
            }

        }
    }

    

    public void AnimationControl() {
        Sprite[] currentSet = downWalk;

        // Determine the current animation set and flip the sprite if needed.
        switch (direction) {
            case "down":
                currentSet = downWalk;
                break;
            case "left":
                transform.localScale = new Vector3(1, 1, 1);
                currentSet = sideWalk;
                break;
            case "right":
                transform.localScale = new Vector3(-1, 1, 1);
                currentSet = sideWalk;
                break;
            case "up":
                currentSet = upWalk;
                break;
        }

        if (directionInput != Vector3.zero || moving) {
            // Update the timer and advance the frame if needed.
            animationTimer += Time.deltaTime;

            if (animationTimer >= frameDuration) {
                animationTimer = 0f;
                // Cycle through frames: 0 -> 1 -> 0 -> 2 -> repeat.
                currentFrame = (currentFrame + 1) % 4;
            }



            // Set the sprite based on the current frame.
            sr.sprite = currentSet[currentFrame];
        } else {
            // When the player is stationary, always show the first frame.
            sr.sprite = currentSet[0];
            animationTimer = 0f; // Reset the animation timer.
            currentFrame = 0; // Reset the frame to the first one.
        }
    }

    public void SetActiveController(bool newState, Vector3 newPosition) {
        isActiveController = newState;

        if (newState) {
            // Update the position when transitioning from 3D.
            newPosition = new Vector3(
                Mathf.Floor(newPosition.x) + 0.5f,
                0,
                Mathf.Floor(newPosition.z) + 0.5f
            );

            // Adjust new position to retain sprite's Y value. 
            newPosition = new Vector3(newPosition.x, transform.position.y, newPosition.z);
            transform.position = newPosition;
            endPosition = newPosition;
        }
    }

}