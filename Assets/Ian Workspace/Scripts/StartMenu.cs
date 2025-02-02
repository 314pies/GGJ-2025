using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{

    public GameObject CreateRoomButton;
    // Start is called before the first frame update
    void Start()
    {
        // Hode CreateRoomButton since Websocket doesn't support hosting
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            CreateRoomButton.SetActive(false);
        }
    }

    public void OnDiscordButtonClicked()
    {
        Application.OpenURL("https://discord.gg/DjRMwa7Myw");
    }
}
