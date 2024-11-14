using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour {
    public TileMovementController playerTileMovement;
    public FPMovementController playerFPMovement;

    public void SetTileAsActive() {
        playerTileMovement.SetActiveController(true);
        playerFPMovement.SetActiveController(false);
    }

    public void SetFPAsActive() {
        playerTileMovement.SetActiveController(false);
        playerFPMovement.SetActiveController(true);
    }
}
