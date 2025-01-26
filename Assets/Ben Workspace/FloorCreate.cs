using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCreate : MonoBehaviour
{

    public GameObject bubblePrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                float xOff = Random.Range(-0.2f, 0.2f);
                float zOff = Random.Range(-0.2f, 0.2f);
                float yOff = Random.Range(-0.2f, 0.2f);
                Instantiate(bubblePrefab, transform.position + new Vector3(i + xOff, 0 + yOff, j + zOff), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
