using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : Changeable
{
    public bool open;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator.SetBool("Open", open);
    }
    
}
