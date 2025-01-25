using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBubble : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionExit(Collision other) {
        Destroy(gameObject);
    }

    void OnCollisionStay(Collision other) {
        Destroy(gameObject, 0.1f);
    }
}
