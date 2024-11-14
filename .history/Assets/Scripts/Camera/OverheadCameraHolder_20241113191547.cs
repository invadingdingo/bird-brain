using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadCameraHolder : MonoBehaviour
{
    public GameObject target;
    
    void FixedUpdate()
    {
        Vector3 fpPosition = fPCharacter.transform.position;

        transform.position = new Vector3(fpPosition.x, 20, fpPosition.z);
    }
}
