using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameStateManager : NetworkBehaviour
{

    public enum GameState
    {
        UnInitiated,
        Wait,
        InGame,
        GameOver
    }

    [SyncVar(hook = nameof(OnGameStateUpdate))]
    public GameState gameState = GameState.UnInitiated;

    [Header("UI GameObjects")]
    public GameObject waitingUI;
    public GameObject gameOverUI;

    [Header("ServerWait")]
    public GameObject gameStartButton;

    [Header("GameOverSyncVar")]
    [SyncVar(hook = nameof(OnWinnerNameupdate))]
    public string winnerName;

    [Header("Other")]
    public GameObject waitFloor;

    public GameObject ServerRestartButton;

    public void ServerStartGame()
    {
        Debug.Log("Server Start Game");
        gameState = GameState.InGame;
    }

    public void ServerReloadScene()
    {
        NetworkManager.singleton.ServerChangeScene("arena");
    }


    public void OnWinnerNameupdate(string oldValue, string newValue)
    {
        gameOverUI.GetComponent<GameOverUI>().updateStatus(newValue);
    }
    public void OnGameStateUpdate(GameState old, GameState newState)
    {
        Debug.Log("OnGameStateUpdate: " + newState);
        waitingUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameStartButton.SetActive(false);
        ServerRestartButton.SetActive(false);
        Time.timeScale = 1.0f;
        switch (newState)
        {
            case GameState.Wait:
                waitingUI.SetActive(true);
                if (isServer)
                {
                    gameStartButton.SetActive(true);
                }
                waitFloor.SetActive(true);
                break;
            case GameState.InGame:
                waitFloor.SetActive(false);
                break;
            case GameState.GameOver:
                gameOverUI.SetActive(true);
                if (isServer)
                {
                    ServerRestartButton.SetActive(true);
                    GameObject.FindGameObjectWithTag("GlobalCamera").GetComponent<FlyCamera>().enabled = false;

                }
                Time.timeScale = 0.5f;
                break;
            default:
                break;
        }
    }

    public override void OnStartServer()
    {
        gameState = GameState.Wait;
    }

    public void StartGame()
    {
        gameState = GameState.InGame;
        RpcGameStartAnnouncement();
    }

    [ClientRpc]
    public void RpcGameStartAnnouncement()
    {
        // Announce Game Start
        Debug.Log("GameStartAnnouncement");
    }

    // Called by Player script
    public void ServerOnClientFallEvent(GameObject player)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int livePlayer = 0;
        GameObject livePlayerObj = player;
        foreach (GameObject p in players)
        {
            if (p.GetComponent<Player>().playerState == (int)Player.PlayerState.ALIVE)
            {
                livePlayer++;
                livePlayerObj = p;
            }
        }
        RpcAnnounceFall(livePlayer);
        if (livePlayer <= 1)
        {
            gameState = GameState.GameOver;
            winnerName = "" + livePlayerObj.GetComponent<NetworkIdentity>().connectionToClient.connectionId;
        } 
    }

    [SerializeField]
    PlayerFallAnnouncement playerFallAnnouncement;

    [ClientRpc]
    public void RpcAnnounceFall(int playerLeft)
    {
        playerFallAnnouncement.gameObject.SetActive(true);
        playerFallAnnouncement.AnnouncePlayerFall(playerLeft);
    }

}
