using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public GameObject bubblePrefab;
    public float avgDistance;
    public float horizontalRange;
    public float verticalRange;
    public float scaleRange;
    public int width;

    void Start()
    {
        for (int i = -width; i < width; i++) {
            for (int j = -width; j < width; j++) {
                float xOff = Random.Range(-horizontalRange, horizontalRange);
                float zOff = Random.Range(-horizontalRange, horizontalRange);
                float yOff = Random.Range(-verticalRange, verticalRange);
                float scale = Random.Range(1 - scaleRange, 1 + scaleRange);

                GameObject bubble = Instantiate(bubblePrefab,
                    transform.position + new Vector3(i * avgDistance + xOff, yOff, j * avgDistance + zOff),
                    Quaternion.identity);

                bubble.transform.localScale *= scale;
            }
        }
    }
}
