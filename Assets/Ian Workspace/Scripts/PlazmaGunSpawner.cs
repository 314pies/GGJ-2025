using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazmaGunSpawner : NetworkBehaviour
{
    public GameObject plazmaGun;

    public Transform[] spawnPoints;
    public float spawnRadious = 15, spawnHeight = 1.0f;

    IEnumerator spawn()
    {
        while(true)
        {
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
            }
            yield return new WaitForSeconds(60.0f);
        }
    }

    public override void OnStartServer()
    {
        StartCoroutine(spawn());
    }
}
