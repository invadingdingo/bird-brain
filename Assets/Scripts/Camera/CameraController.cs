using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform fPCameraHolder, overheadCameraHolder;

    public ControlManager cm;

    public bool overhead;
    private Camera camera;
    private Quaternion storedRotation;
    public PostProcessLayer outlineShader;
    public PostProcessVolume outlineVolume;

    private bool canTab = true;
    
    

    void Awake()
    {
        camera = GetComponent<Camera>();
        
    }

    void Start()
    { 
        overhead = !overhead;
        SwitchHolders();
        StartCoroutine("DumbProcessing");
    }

    public void SetCanTab(bool canWe)
    {
        canTab = canWe;
    }

    private IEnumerator DumbProcessing()
    {
        yield return new WaitForFixedUpdate();
        overhead = !overhead;
        SwitchHolders();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && canTab)
        {
            overhead = !overhead;
            SwitchHolders();
        }
    }

    void SwitchHolders()
    {
        if (overhead)
        {
            transform.SetParent(overheadCameraHolder,false);
            camera.orthographic = true;
            transform.localRotation = Quaternion.identity;
            camera.cullingMask = camera.cullingMask |= (1 << 7);
            camera.cullingMask = camera.cullingMask &= ~(1 << 6);
            
            //Turn off outline shader
            outlineShader.enabled = false;
            outlineVolume.enabled = false;

            // Change music
            AudioManager.instance.SwitchMusic("top");

            // Tells control manager to switch to top down controls. 
            cm.SetTileAsActive();
            
        }
        else
        {
            transform.SetParent(fPCameraHolder,false);
            camera.orthographic = false;
            transform.localRotation = storedRotation;
            camera.cullingMask = camera.cullingMask |= (1 << 6);
            camera.cullingMask = camera.cullingMask &= ~(1 << 7);

            //Turn on outline shader
            outlineShader.enabled = true;
            outlineVolume.enabled = true;

            // Change music
            AudioManager.instance.SwitchMusic("first");
            
            // Tells control manager to switch to first person controls. 
            cm.SetFPAsActive();
        }
    }
}
