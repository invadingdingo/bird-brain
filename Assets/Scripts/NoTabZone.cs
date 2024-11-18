using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTabZone : MonoBehaviour
{
    private CameraController camController;
    
    void Awake()
    {
        camController = FindObjectOfType<CameraController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            camController.SetCanTab(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            camController.SetCanTab(true);
        }
    }
}
