using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadlessManager : MonoBehaviour
{
    public bool forceStartServer = false;
    // Start is called before the first frame update
    void Start()
    {
        if(Headless.IsHeadless() || forceStartServer)
        {
            GetComponent<NetworkManager>().StartServer();
        }
    }
}
