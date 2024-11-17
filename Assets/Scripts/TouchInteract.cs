using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInteract : MonoBehaviour
{
    private Animator myAnimator;

    public List<Changeable> controls;

    public TouchInteract otherPerspective;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (otherPerspective != null)
                otherPerspective.OtherPerspectiveTrigger();
            if (myAnimator != null)
                myAnimator.SetTrigger("Trigger");
            foreach(Changeable control in controls)
                control.Change();
        }
    }

    public void OtherPerspectiveTrigger()
    {
        if (myAnimator != null)
            myAnimator.SetTrigger("Trigger");
        if (controls.Count>0)
        {
            foreach(Changeable control in controls)
                control.Change();
        }
        
    }
}
