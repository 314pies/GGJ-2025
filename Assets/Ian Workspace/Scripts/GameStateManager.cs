using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class GameStateManager : NetworkBehaviour
{
    public const string GameStateManagerTag = "GameStateManager";

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
    public TMP_Text playerReadyStatus;
    public GameObject gameOverUI;
    

    [Header("GameOverSyncVar")]
    [SyncVar(hook = nameof(OnWinnerNameupdate))]
    public string winnerName;
    [SyncVar(hook = nameof(OnGameRestartCountdownUpdate))]
    public string gameRestartTextString;
    public TMP_Text gameRestartText;

    [Header("Other")]
    public GameObject waitFloor;

    [SyncVar(hook = nameof(OnPlayerReadyStatusUpdate))]
    public string playerWaitingStatus;



    public void ServerStartGame()
    {
        Debug.Log("Server Start Game");
        gameState = GameState.InGame;
        RpcGameStartAnnouncement();
    }

    public void ServerReloadScene()
    {
        NetworkManager.singleton.ServerChangeScene("arena");
    }


    public void OnWinnerNameupdate(string oldValue, string newValue)
    {
        gameOverUI.GetComponent<GameOverUI>().updateStatus(newValue);
    }

    public void OnGameRestartCountdownUpdate(string oldValue, string newValue)
    {
        gameRestartText.text = newValue;
    }

    public void OnGameStateUpdate(GameState old, GameState newState)
    {
        Debug.Log("OnGameStateUpdate: " + newState);
        waitingUI.SetActive(false);
        gameOverUI.SetActive(false);

        Time.timeScale = 1.0f;
        switch (newState)
        {
            case GameState.Wait:
                waitingUI.SetActive(true);
                waitFloor.SetActive(true);
                break;
            case GameState.InGame:
                waitFloor.SetActive(false);
                break;
            case GameState.GameOver:
                gameOverUI.SetActive(true);
                Time.timeScale = 0.5f;
                break;
            default:
                break;
        }
    }

    public void OnPlayerReadyStatusUpdate(string oldValue, string newValue)
    {
        playerReadyStatus.text = newValue;
    }

    public override void OnStartServer()
    {
        gameState = GameState.Wait;
        StartCoroutine(ServerUpdatePlayerReadyStatePolling());
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
            StartCoroutine(ServerRestartCountDown());
        }
    }

    IEnumerator ServerRestartCountDown()
    {
        int countDownSeconds = 20;
        while (countDownSeconds > 0) {
            gameRestartTextString = "Game Restart in..." + countDownSeconds;
            yield return new WaitForSecondsRealtime(1);
            countDownSeconds--;
        }
        ServerReloadScene();
    }

    [SerializeField]
    PlayerFallAnnouncement playerFallAnnouncement;

    [ClientRpc]
    public void RpcAnnounceFall(int playerLeft)
    {
        playerFallAnnouncement.gameObject.SetActive(true);
        playerFallAnnouncement.AnnouncePlayerFall(playerLeft);
    }

    public IEnumerator ServerUpdatePlayerReadyStatePolling()
    {
        while (gameState == GameState.Wait) {            
            ServerUpdatePlayerReadyState();
            yield return new WaitForSeconds(1.5f);
        }
    }

    public void ServerUpdatePlayerReadyState()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int readyPlayerCount = 0;
        foreach (GameObject p in players)
        {
            if (p.GetComponent<Player>().isReadyToStart)
            {
                readyPlayerCount++;
            }
        }

        playerWaitingStatus = "Waiting for all players to get ready\n ("
            + readyPlayerCount + "/" + players.Length + ")";

        if (readyPlayerCount == players.Length && players.Length >= 1)
        {
            // Everyone ready, start game
            ServerStartGame();
        }
    }

}
