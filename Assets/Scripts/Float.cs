using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{

    public float floatSpeed = 1f;
    private float actualFloatSpeed;

    // Start is called before the first frame update
    void Start()
    {
        actualFloatSpeed = floatSpeed * Random.Range(0.75f, 1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * actualFloatSpeed * Time.deltaTime);
    }
}
