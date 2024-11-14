using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour {
    public GameObject playerTileMovement;
    public GameObject playerFPMovement;

    public void SetTileAsActive() {
        playerTileMovement.SetActiveController(true, playerFPMovement.position);
        playerFPMovement.SetActiveController(false, Vector3.zero);
    }

    public void SetFPAsActive() {
        playerTileMovement.SetActiveController(false, playerTileMovement.position);
        playerFPMovement.SetActiveController(true, Vector3.zero);
    }
}
