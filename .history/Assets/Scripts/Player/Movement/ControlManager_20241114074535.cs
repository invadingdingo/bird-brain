using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour {
    public TileMovementController playerTileMovement;
    public FPMovementController playerFPMovement;

    public void SetTileAsActive() {
        playerTileMovement.SetActiveController(true, playerFPMovement.position);
        playerFPMovement.SetActiveController(false);
    }

    public void SetFPAsActive() {
        playerTileMovement.SetActiveController(false), playerTileMovement.position;
        playerFPMovement.SetActiveController(true);
    }
}
