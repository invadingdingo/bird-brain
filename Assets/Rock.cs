using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    public GameObject thirdPersonRock;
    private Vector3 startPosition;

    [SerializeField] private LayerMask collision;

    [SerializeField] private float pushTime = 0.5f;
    private bool beingPushed = false;
    private Vector3 endPosition;

    void Start() {
        startPosition = transform.position;
    }

    public void Push(string direction) {

        // If the rock isn't currently being pushed,
        if (!beingPushed) {

            // Set the end position based on the direction given.
            switch (direction) {
                case "up":
                    endPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                    break;
                case "down":
                    endPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f);
                    break;
                case "left":
                    endPosition = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
                    break;
                case "right":
                    endPosition = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
                    break;
            }

            // If the end position isn't occupied..
            if (!IsPositionOccupied(endPosition)) {

                // Lerp the position of the rock to the end position.
                StartCoroutine(LerpPosition(endPosition));
            
            } else {
                Debug.Log("The position is occupied..");
            }

        }
    }
    
    // Coroutine to lerp the rock's position
    private IEnumerator LerpPosition(Vector3 targetPosition) {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        beingPushed = true;

        while (elapsedTime < pushTime) {
            // Interpolate position based on elapsed time
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / pushTime);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the position is exactly the target at the end
        transform.position = targetPosition;
        beingPushed = false;
    }

    public void Reset() {
        transform.position = startPosition;
    }

    private bool IsPositionOccupied(Vector3 position) {
        Vector3 checkSize = new Vector3(0.5f, 0f, 0.5f);
        bool isOccupied = Physics.CheckBox(position, checkSize / 2, Quaternion.identity, collision);
        return isOccupied;
    }
}
