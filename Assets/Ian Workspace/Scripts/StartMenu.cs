using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public const string DISCORD_LINK = "https://discord.gg/W8bvVuuTvQ";

    public static string InputPlayerName = "Player X";
    const string PLAYER_NAME_SAVE_KEY = "SWA_PLAYER_NAME";
    public TMP_InputField nameInputField;

    public GameObject CreateRoomButton;
    // Start is called before the first frame update
    void Start()
    {
        // Hide CreateRoomButton since Websocket doesn't support hosting
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            CreateRoomButton.SetActive(false);
        }

        nameInputField.text = PlayerPrefs.GetString(PLAYER_NAME_SAVE_KEY);
    }

    public void UpdateLocalName()
    {
        InputPlayerName = nameInputField.text;
        PlayerPrefs.SetString(PLAYER_NAME_SAVE_KEY, InputPlayerName);
    }

    public void OnDiscordButtonClicked()
    {
        Application.OpenURL(DISCORD_LINK);
    }
}
