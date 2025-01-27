using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class StartMenuNetworking : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject StarMenuUI;
    public Camera startMenuCamera;

    public override void OnStartClient()
    {
        base.OnStartClient();

        OnConnectedToServer(false);
    }

    void OnConnectedToServer(bool status)
    {
        if (StarMenuUI != null)
            StarMenuUI.SetActive(status);
        if (startMenuCamera != null)
            startMenuCamera.enabled = status;
            startMenuCamera.GetComponent<AudioSource>().Stop();
    }

    public override void OnStopClient()
    {
        OnConnectedToServer(true);
    }
}
