using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInteract : MonoBehaviour
{
    private Animator myAnimator;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Change Now!");
            myAnimator.SetTrigger("Trigger");
        }
    }
}
