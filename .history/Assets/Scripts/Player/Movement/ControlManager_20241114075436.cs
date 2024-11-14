using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour {
    public GameObject playerTileMovement;
    public GameObject playerFPMovement;

    public void SetTileAsActive() {
        playerTileMovement.GetComponent<TileMovementController>().SetActiveController(true, playerFPMovement.transform.position);
        playerFPMovement.GetComponent<FPMovementController>().SetActiveController(false, Vector3.zero);
    }

    public void SetFPAsActive() {
        playerTileMovement.GetComponent<TileMovementController>().SetActiveController(false, playerTileMovement.transform.position);
        playerFPMovement.GetComponent<FPMovementController>().SetActiveController(true, Vector3.zero);
    }
}
