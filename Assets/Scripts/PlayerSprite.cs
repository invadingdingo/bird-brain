using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    private CharacterController charController;

    void Awake()
    {
        charController = FindObjectOfType<CharacterController>();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        var position = charController.transform.position;
        transform.position = new Vector3(position.x, 0, position.z);
    }
}
