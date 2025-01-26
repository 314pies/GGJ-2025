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

        OnConnectedToServer();
    }

    void OnConnectedToServer()
    {
        StarMenuUI.SetActive(false);
        startMenuCamera.enabled = false;
    }
}
