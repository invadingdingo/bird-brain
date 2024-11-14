using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class TileMovementController : MonoBehaviour {

    [SerializeField] private float distanceToMove;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Sprite[] spriteSheet;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator anim;

    // Directional rays to check each cardinal direction from player.
    private RaycastHit2D downCast;
    private RaycastHit2D upCast;
    private RaycastHit2D rightCast;
    private RaycastHit2D leftCast;

    private bool moveToPoint = false;
    private Vector3 endPosition;
    private bool moving;
    private int direction;
    private Vector3 prevPos;

    private Vector3 facing;

    private bool canMove;
    private GameObject interactable;

    void Start() {
        endPosition = transform.position;
        moving = false;
        canMove = true;
        prevPos = transform.position;

    }

    void Update() {
    	if (canMove) {
    		HandleMovement();
    	} else {
    		canMove = !interactable.GetComponent<NPC>().interacting;
    	}
    }

    void FixedUpdate() {
        if (moveToPoint) {
            endPosition = new Vector3(endPosition.x, endPosition.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);

            if (transform.position != endPosition) {
                moving = true;
            } else {
                moving = false;
            }

        }

        bool stillMoving = (transform.position != prevPos);
        anim.SetBool("moving", stillMoving);
        anim.SetInteger("direction", direction);

        prevPos = transform.position;

    }

    private void HandleMovement() {
    	downCast = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - distanceToMove), -Vector2.up, distanceToMove);
        upCast = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - distanceToMove), Vector2.up, distanceToMove);
        leftCast = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - distanceToMove), -Vector2.right, distanceToMove);
        rightCast = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - distanceToMove), Vector2.right, distanceToMove);

        Vector3 directionInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        interactable = CheckInteract(facing);
        if (interactable != null) {
        	if (Input.GetButtonDown("Interact")) {
        		canMove = false;
        		interactable.GetComponent<NPC>().Interact();
        	}
        }
        
        if (directionInput != Vector3.zero && !moving) {
            if (directionInput.y == -1) {
                if (downCast.collider == null) { endPosition = new Vector3(transform.position.x, transform.position.y - distanceToMove, transform.position.z); }
                direction = 0;
                sr.sprite = spriteSheet[0];
                facing = new Vector3(0, -1, 0);
            } else if (directionInput.x == -1) {
                if (leftCast.collider == null) { endPosition = new Vector3(transform.position.x - distanceToMove, transform.position.y, transform.position.z); }
                direction = 1;
                sr.sprite = spriteSheet[4];
                facing = new Vector3(-1, 0, 0);
            } else if (directionInput.x == 1) {
                if (rightCast.collider == null) { endPosition = new Vector3(transform.position.x + distanceToMove, transform.position.y, transform.position.z); }
                direction = 2;
                sr.sprite = spriteSheet[8];
                facing = new Vector3(1, 0, 0);
            } else if (directionInput.y == 1) {
                if (upCast.collider == null) { endPosition = new Vector3(transform.position.x, transform.position.y + distanceToMove, transform.position.z); }
                direction = 3;
                sr.sprite = spriteSheet[12];
                facing = new Vector3(0, 1, 0);
            }
           	moveToPoint = true;
        }
    }

    private GameObject CheckInteract(Vector3 facing) {
    	RaycastHit2D interactCast = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - distanceToMove), facing, distanceToMove);
    	if (interactCast.collider != null) {
	    	if (interactCast.collider.tag == "Interactable") {
	    		return interactCast.collider.gameObject;
	    	}
	    }
	    return null;
    }

}