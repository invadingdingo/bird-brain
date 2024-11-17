using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changeable : MonoBehaviour
{
    public Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }
    public virtual void Change()
    {
        myAnimator.SetTrigger("Trigger");
    }
}
