using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class BubblePopper : NetworkBehaviour
{
    [SyncVar]
    [SerializeField] public bool power = false;
    public AudioClip popSound;

    public bool disableDestroy = false;

    float destroyDelay = 0.4f;

    Collider[] hitColliders = new Collider[100];

    public float radious = 0.5f;

    void FixedUpdate()
    {
        if (!power)
        {
            //Collider[] hitColliders = Physics.OverlapCapsule(transform.position, transform.position + Vector3.down, 0.5f);

            Physics.OverlapCapsuleNonAlloc(transform.position, transform.position + Vector3.down, radious, hitColliders);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider == null)
                {
                    continue;
                }
                FloorBubble floorBubble = hitCollider.gameObject.GetComponent<FloorBubble>();

                if (floorBubble != null)
                {
                    //floorBubble.PopEffect();
                    if (isServer)
                    {
                        if (!floorBubble.destroying)
                        {
                            floorBubble.destroying = true;
                            if (!disableDestroy)
                            {
                                StartCoroutine(PlayPop(floorBubble.transform.position));
                                Destroy(floorBubble.gameObject, destroyDelay);
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator PlayPop(Vector3 position)
    {
        yield return new WaitForSeconds(destroyDelay);
        AudioSource.PlayClipAtPoint(popSound, position);
    }
}
