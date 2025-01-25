using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject bubblePrefab;

    void Start()
    {

        for (int i = 0; i < 20; i++) {
            for (int j = 0; j < 20; j++) {
                float xOff = Random.Range(-0.2f, 0.2f);
                float zOff = Random.Range(-0.2f, 0.2f);
                float yOff = Random.Range(-0.2f, 0.2f);
                Instantiate(bubblePrefab, new Vector3(i * 0.8f + xOff, 0 + yOff, j * 0.8f + zOff), Quaternion.identity);
            }
        }
    }

    void Update()
    {
        
    }
}
