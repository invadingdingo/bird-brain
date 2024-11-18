using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockResetButton : MonoBehaviour {

    public GameObject[] rocksToReset;

    void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player")) {
            
            // Play button SFX.
            AudioManager.instance.PlayButton();

            foreach (GameObject rock in rocksToReset) {
                rock.GetComponent<Rock>().Reset();
            }
        }
    }
    
}
