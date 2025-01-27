using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class StartMenuNetworking : NetworkBehaviour
{
    public GameObject StarMenuUI;
    public Camera startMenuCamera;

    public override void OnStartClient()
    {
        OnConnectionStatusChanged(false);
    }

    public override void OnStopClient()
    {
        OnConnectionStatusChanged(true);
    }

    void OnConnectionStatusChanged(bool status)
    {
        if (StarMenuUI != null)
        {
            StarMenuUI.SetActive(status);
        }

        if (startMenuCamera != null)
        {
            startMenuCamera.enabled = status;
            startMenuCamera.GetComponent<AudioSource>().enabled = status;
            startMenuCamera.GetComponent<AudioListener>().enabled = status;
        }
    }
}
