using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public const string DISCORD_LINK = "https://discord.gg/W8bvVuuTvQ";

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
        Application.OpenURL(DISCORD_LINK);
    }
}
