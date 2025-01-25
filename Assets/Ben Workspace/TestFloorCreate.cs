using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFloorCreate : MonoBehaviour
{

    public GameObject bubblePrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                float xOff = Random.Range(-0.2f, 0.2f);
                float zOff = Random.Range(-0.2f, 0.2f);
                float yOff = Random.Range(-0.2f, 0.2f);
                Instantiate(bubblePrefab, new Vector3(i * 0.8f + xOff, 0 + yOff, j * 0.8f + zOff), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
