using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinGame : MonoBehaviour
{

    public NetworkManager networkManager;
    public TMP_InputField ipInputField;
    public Button joinGameButton;
    public TMP_Text statusText;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.singleton;

        joinGameButton.onClick.AddListener(joinGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void joinGame()
    {
        string ipAddress = ipInputField.text;  // Get the IP address entered by the player

        if (!string.IsNullOrEmpty(ipAddress))
        {
            ConnectToServer(ipAddress);
        }
        else
        {
            SetStatusText("Please enter a valid IP address.");
        }
    }

    void ConnectToServer(string ipAddress)
    {
        // Display connecting message
        SetStatusText("Connecting to server...");

        // Start the client and connect to the provided IP address
        NetworkManager.singleton.networkAddress = ipAddress;  // Set the IP address of the server
        NetworkManager.singleton.StartClient();  // Start the client to connect to the server
    }

    void SetStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}
