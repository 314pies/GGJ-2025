using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Float : NetworkBehaviour
{
    public float baseFlatSpeed = 2.5f;
    public float topBeforePop = 100f;
    public float timeBeforePopSeconds = 40f;
    [SyncVar]
    private float actualFloatSpeed;
    [SyncVar]
    private float actualTimeBeforePop;
    [SyncVar]
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        if (!isServer)
        {
            return;
        }

        actualFloatSpeed = baseFlatSpeed * Random.Range(0.75f, 1.25f);
        actualTimeBeforePop = timeBeforePopSeconds * Random.Range(0.75f, 1.25f);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        if ((transform.position.y > topBeforePop) || (Time.time - startTime > actualTimeBeforePop))
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector3.up * actualFloatSpeed * Time.deltaTime);
    }

    public enum CollisionEnum
    {
        DESTROY_BOTH,
        DESTROY_BOTTOM,
    }
    public CollisionEnum collisionType = CollisionEnum.DESTROY_BOTH;
    public float secondsBeforePop = 0.1f;
    private float timeOfFirstCollision = -1f;

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (player.isLocalPlayer) {
                GetComponent<AudioSource>().Play();
                bouncePlayer(player);
                cmdDestroyBubble();
            }            
        }
    }
    [Command(requiresAuthority = false)]
    public void cmdDestroyBubble()
    {
        StartCoroutine(handlePlayerCollision());
    }


    private void OnCollisionStay(Collision collision)
    {
        if (!isServer)
        {
            return;
        }

        if (timeOfFirstCollision < 0f)
        {
            timeOfFirstCollision = Time.time;
        }
        float timePastFirstCollision = Time.time - timeOfFirstCollision;
        bool shouldDestroy = (timePastFirstCollision > secondsBeforePop);
        Player player = collision.gameObject.GetComponent<Player>();

        if (shouldDestroy)
        {
            switch (collisionType)
            {
                case CollisionEnum.DESTROY_BOTH:
                    Destroy(gameObject);
                    if (isDeletableCollision(collision.gameObject))
                    {
                        Destroy(collision.gameObject);
                    }
                    break;
                case CollisionEnum.DESTROY_BOTTOM:
                    Destroy(gameObject);
                    break;
                default:
                    throw new System.Exception("Invalid collision type " + collisionType.ToString());
            }
        }
    }

    private bool isDeletableCollision(GameObject gameObject)
    {
        return gameObject.GetComponent<FloorBubble>() != null || gameObject.GetComponent<FloorBubble>() != null;
    }

    IEnumerator handlePlayerCollision()
    {
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);   
    }

    private void bouncePlayer(Player player)
    {
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * 300, ForceMode.Impulse);
    }
}
