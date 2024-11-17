using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInteract : MonoBehaviour
{
    private Animator myAnimator;

    public List<Changeable> controls;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myAnimator.SetTrigger("Trigger");
            foreach(Changeable control in controls)
                control.Change();
        }
    }
}
