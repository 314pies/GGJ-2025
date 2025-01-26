using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePopper : NetworkBehaviour
{
    [SyncVar]
    [SerializeField] public bool power = false;
    void FixedUpdate()
    {
        if (!power)
        {
            Collider[] hitColliders = Physics.OverlapCapsule(transform.position, transform.position + Vector3.down, 0.5f);
            foreach (var hitCollider in hitColliders)
            {
                FloorBubble floorBubble = hitCollider.gameObject.GetComponent<FloorBubble>();

                if (floorBubble != null)
                {
                    floorBubble.Pop();
                }
            }
        }
    }
}
