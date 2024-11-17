using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadCameraHolder : MonoBehaviour
{
    public GameObject target;
    
    void FixedUpdate()
    {
        transform.position = new Vector3(target.transform.position.x, 60, target.transform.position.z);
    }
}
