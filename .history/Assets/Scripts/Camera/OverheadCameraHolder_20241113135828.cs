using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadCameraHolder : MonoBehaviour
{
    public CharacterController fPCharacter;

    void Awake()
    {
        fPCharacter = FindObjectOfType<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        Vector3 fpPosition = fPCharacter.transform.position;

        transform.position = new Vector3(fpPosition.x, 20, fpPosition.z);
    }
}
