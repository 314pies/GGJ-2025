using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class BubblePopper : NetworkBehaviour
{
    [SyncVar]
    [SerializeField] public bool power = false;

    public bool disableDestroy = false;
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
                    //floorBubble.PopEffect();
                    if (isServer)
                    {
                        if (!floorBubble.destroying)
                        {
                            if (!disableDestroy)
                            {
                                floorBubble.destroying = true;

                                Destroy(floorBubble.gameObject, 0.4f);
                            }
                        }
                    }
                }
            }
        }
    }
}
