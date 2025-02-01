using System.Collections;
using System.Collections.Generic;
using TextAnimation;
using UnityEngine;

public class TypeWriteTest : MonoBehaviour
{

    public Typewriter typewriter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            typewriter.StartTypewriter();
        }
    }
}
