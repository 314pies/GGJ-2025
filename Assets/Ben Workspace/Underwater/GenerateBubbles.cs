using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GenerateBubbles : NetworkBehaviour
{

    public GameObject floatingBubblePrefab;

    // Between 0-1 for probability, per second
    public float generatorThreshold = 5f;
    public float xMax = 25f;
    public float zMax = 25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        float actualThreshold = generatorThreshold * Time.deltaTime;
        bool shouldGenerate = Random.value < actualThreshold;
        if (shouldGenerate)
        {
            float xOff = Random.Range(-xMax, xMax);
            float zOff = Random.Range(-zMax, zMax);
            GameObject obj = Instantiate(floatingBubblePrefab, transform.position + new Vector3(xOff, 0, zOff), Quaternion.identity);
            NetworkServer.Spawn(obj);
        }
    }
}
