using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameTag : MonoBehaviour
{
    public Camera renderingCamera;
    public TMP_Text nameTagUI;

    private void OnEnable()
    {
        if (Headless.IsHeadless()) { 
            enabled = false;
            Debug.Log("Disabling PlayerNameTag on Headless Server");
            return;
        }
        SearchCamera();
    }

    public void SetNameTag(string name)
    {
        nameTagUI.text = name;
    }

    const float maxSearchInterval = 0.15f;
    private float lastSearch = 0.0f;
    private void SearchCamera()
    {
        if (Time.time < lastSearch + maxSearchInterval) {
            return;
        }
        lastSearch = Time.time;

        // Find main Camera first
        renderingCamera = Camera.main;

        // Fallback to Global Camera
        GameObject globalCamObj = GameObject.FindWithTag("GlobalCamera");
        if (renderingCamera == null && globalCamObj != null && globalCamObj.GetComponent<Camera>().isActiveAndEnabled) {
            renderingCamera = globalCamObj.GetComponent<Camera>();
            Debug.LogWarning("PlayerNameTag: Main camera not found, falling back to GlobamCamera");
        }

        if (renderingCamera == null) {
            Debug.LogWarning("PlayerNameTag, Camera not found.");
        } else
        {
            Debug.Log("PlayerNameTag, Main Cam: " + renderingCamera.name);
        }
    }

    private void LateUpdate()
    {
        if (renderingCamera != null && renderingCamera.isActiveAndEnabled) {
            transform.LookAt(renderingCamera.transform);
            transform.RotateAround(transform.position, transform.up, 180f);
        } else
        {
            SearchCamera();
        }
    }
}
