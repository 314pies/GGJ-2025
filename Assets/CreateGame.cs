using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CreateGame : MonoBehaviour
{
    public NetworkManager networkManager;
    public TMP_Text statusText;
    public Button createGameButton;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.singleton;
        createGameButton.onClick.AddListener(onCreateGameClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onCreateGameClicked()
    {
        networkManager.StartHost();
        SetStatusText("Creating host...");
    }

    void SetStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}
