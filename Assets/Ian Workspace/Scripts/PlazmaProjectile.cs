using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazmaProjectile : NetworkBehaviour
{
    public float explodeRadious = 3.0f;
    public float explodeRadiousToPlayer = 5.0f;
    public const string FLOOR_TAG = "Floor", PLAYER_TAG = "Player";

    public GameObject flyingPlazma, explosion;

    public float explosiceYOffset = -0.5f;

    public float PlayerPushForce = 100.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer) { return; }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explodeRadious);
        foreach (var hitCollider in hitColliders)
        {
           if (hitCollider.gameObject.tag == FLOOR_TAG)
            {
                Destroy(hitCollider.gameObject);
            }
        }
        GetComponent<Rigidbody>().isKinematic = true;
        RpcExplode();
        Destroy(gameObject, 10.0f);
    }

    [ClientRpc]
    public void RpcExplode()
    {
        
        flyingPlazma.SetActive(false);
        explosion.transform.position = new Vector3(
            explosion.transform.position.x,
            explosion.transform.position.y + explosiceYOffset,
            explosion.transform.position.z
            );
        explosion.transform.LookAt(transform.position + Vector3.up, Vector3.up);
        explosion.SetActive(true);
        GetComponent<AudioSource>().Play();
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;

        // Push away player
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explodeRadiousToPlayer);
        foreach (var hitCollider in hitColliders)
        {           
            if (hitCollider.gameObject.tag == PLAYER_TAG)
            {
                GameObject player = hitCollider.gameObject;
                if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    Vector3 forceDir = player.transform.position - transform.position;
                    player.GetComponent<Rigidbody>().AddForce(forceDir.normalized * PlayerPushForce, ForceMode.VelocityChange);
                    break;
                }
            }
        }
    }

    //void OnDrawGizmosSelected()
    //{
    //    // Draw a red sphere at the transform's position
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, explodeRadious);
    //}
}
