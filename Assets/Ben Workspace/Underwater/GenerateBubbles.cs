using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GenerateBubbles : NetworkBehaviour
{

    public GameObject floatingBubblePrefab;

    // Between 0-1 for probability, per second
    public float generatorThreshold = 0.2f;
    public float xMax = 25f;
    public float zMax = 25f;

    GameStateManager gameStateManager;

    public override void OnStartServer()
    {
        gameStateManager = GameObject.FindGameObjectWithTag(GameStateManager.GameStateManagerTag)
                        .GetComponent<GameStateManager>();

        InvokeRepeating("ServerSpawnFloagingBubble", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void ServerSpawnFloagingBubble()
    {
        if (!isServer)
        {
            return;
        }

        if (gameStateManager.gameState != GameStateManager.GameState.InGame)
        {
            return;
        }

        bool shouldGenerate = Random.value < generatorThreshold;

        if (shouldGenerate)
        {
            float xOff = Random.Range(-xMax, xMax);
            float zOff = Random.Range(-zMax, zMax);
            Vector3 spawnPos = transform.position + new Vector3(xOff, 0, zOff);
            GameObject obj = Instantiate(floatingBubblePrefab, spawnPos, Quaternion.identity);
            NetworkServer.Spawn(obj);
            Debug.Log("Floating Bubble Spawned at " + spawnPos);
        }
    }
}
