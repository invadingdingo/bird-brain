using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform fPCameraHolder, overheadCameraHolder;

    public ControlManager cm;

    public bool overhead;
    private Camera camera;
    private Quaternion storedRotation;

    void Awake()
    {
        // TEMPORARY! Sets FP as active at start.
        cm.SetFPAsActive();

        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            overhead = !overhead;
            SwitchHolders();
        }
    }

    void SwitchHolders()
    {
        if (overhead)
        {   
            // Tells control manager to switch to top down controls. 
            cm.SetTileAsActive();

            storedRotation = transform.localRotation;
            transform.SetParent(overheadCameraHolder,false);
            camera.orthographic = true;
            transform.localRotation = Quaternion.identity;
            camera.cullingMask = camera.cullingMask |= (1 << 7);
            camera.cullingMask = camera.cullingMask &= ~(1 << 6);
        }
        else
        {
            Debug.Log("setting tile as active");
            // Tells control manager to switch to first person controls. 
            cm.SetFPAsActive();

            transform.SetParent(fPCameraHolder,false);
            camera.orthographic = false;
            transform.localRotation = storedRotation;
            camera.cullingMask = camera.cullingMask |= (1 << 6);
            camera.cullingMask = camera.cullingMask &= ~(1 << 7);
        }
    }
}
