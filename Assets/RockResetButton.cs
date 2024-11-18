using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockResetButton : MonoBehaviour {

    public GameObject[] rocksToReset;

    void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player")) {
            Debug.Log("Entered");
            foreach (GameObject rock in rocksToReset) {
                rock.GetComponent<Rock>().Reset();
            }
        }
    }
    
}
