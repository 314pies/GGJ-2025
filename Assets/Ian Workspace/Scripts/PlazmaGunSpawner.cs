using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameStateManager;

public class PlazmaGunSpawner : NetworkBehaviour
{
    public GameObject plazmaGun;

    public Transform[] spawnPoints;
    public float spawnRadious = 15, spawnHeight = 1.0f;

    IEnumerator spawn()
    {
        while(true)
        {
            if (GameObject.FindGameObjectWithTag(GameStateManager.GameStateManagerTag)
                .GetComponent<GameStateManager>().gameState != GameState.InGame)
            {
                Debug.Log("PlazmaGunSpawner: Game not start yet, skip spawning.");
                yield return new WaitForSeconds(60.0f);
                continue;
            }

            foreach (var sp in spawnPoints)
            {
                float xVar = Random.Range(-spawnRadious, spawnRadious);
                float zVar = Random.Range(-spawnRadious, spawnRadious);

                Vector3 spos = new Vector3(
                    sp.position.x + xVar,
                    sp.position.y + spawnHeight,
                    sp.position.z + zVar
                    );
                GameObject instance = Instantiate(plazmaGun, spos, Quaternion.identity);
                NetworkServer.Spawn(instance);
                Debug.Log("PlazmaGunSpawner: Spawn 1 Plazma gun at " + spos);
            }
            yield return new WaitForSeconds(60.0f);
        }
    }

    public override void OnStartServer()
    {
        StartCoroutine(spawn());
    }
}
