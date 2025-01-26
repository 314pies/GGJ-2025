using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBubble : NetworkBehaviour
{
    public Material shinyMaterial;
    private float popping = -1.0f;

    [SyncVar(hook = nameof(PopEffect))]
    public bool destroying = false;

    void Update() {
        if (popping > 0) {
            gameObject.transform.localScale += 1.2f * Vector3.one * Time.deltaTime;
        }
    }



    public void PopEffect(bool old, bool isDestroying) {
        if (isDestroying)
        {
            popping = Time.time;
            GetComponent<Renderer>().material = shinyMaterial;
            //Destroy(gameObject, 0.4f);
        }
    }
}
