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

    public void OnGameStateUpdate(GameState old, GameState newState)
    {

        waitingUI.SetActive(false);
        gameOverUI.SetActive(false);

        switch (newState)
        {
            case GameState.Wait:
                waitingUI.SetActive(true);
                break;
            case GameState.InGame:
                break;
            case GameState.GameOver:
                gameOverUI.SetActive(true);
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

    public void OnClientFallEvent(GameObject player)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int livePlayer = 0;
        foreach (GameObject p in players)
        {
            //if (p.GetComponent<Player>().status)
        }
    }

}
