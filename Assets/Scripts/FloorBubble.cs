using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBubble : MonoBehaviour
{
    public Material shinyMaterial;
    private float popping;

    void Update() {
        if (popping > 0) {
            gameObject.transform.localScale += 1.2f * Vector3.one * Time.deltaTime;
        }
    }

    public void Pop() {
        //popping = Time.time;
        //GetComponent<Renderer>().material = shinyMaterial;
        //Destroy(gameObject, 0.4f);
    }
}
