using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    public GameObject FPRock;

    private Vector3 TDStartPosition;
    private Vector3 FPStartPosition;

    [SerializeField] private LayerMask TDCollision;
    [SerializeField] private LayerMask FPCollision;

    [SerializeField] private float pushTime = 0.5f;
    private bool beingPushed = false;
    private Vector3 endPosition;

    void Start() {
        TDStartPosition = transform.position;
        FPStartPosition = FPRock.transform.position;
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

        // Play Rock Push SFX.
        AudioManager.instance.PlayRockPush();

        while (elapsedTime < pushTime) {
            // Interpolate position based on elapsed time
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / pushTime);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the position is exactly the target at the end
        transform.position = targetPosition;
        MoveFPRock(targetPosition);
        beingPushed = false;
    }

    void MoveFPRock(Vector3 newPosition) {
        // Raycast to find the height of the ground being transitioned to.
        // Shoot ray down from 30 units up to check for intersection.
        Ray ray = new Ray(new Vector3(newPosition.x, 30f, newPosition.z), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 40f, FPCollision)) {
            // If intersection found, set newPosition.y to the point of intersection plus 1 to account for player height..
            newPosition.y = hit.point.y;
        } else {
            Debug.Log("Ground not found!");
        }

        FPRock.transform.position = newPosition;

    }

    public void Reset() {
        transform.position = TDStartPosition;
        FPRock.transform.position = FPStartPosition;
    }

    private bool IsPositionOccupied(Vector3 position) {
        Vector3 checkSize = new Vector3(0.5f, 0f, 0.5f);
        bool isOccupied = Physics.CheckBox(position, checkSize / 2, Quaternion.identity, TDCollision);
        return isOccupied;
    }
}
