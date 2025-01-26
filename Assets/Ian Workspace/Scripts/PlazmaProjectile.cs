using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazmaProjectile : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
       if (!isServer) { return; }
       
    }

    public float explodeRadious = 3.0f;
    public const string FLOOR_TAG = "Floor";

    public GameObject flyingPlazma, explosion;

    public float explosiceYOffset = -0.5f;

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
    }

    void OnDrawGizmosSelected()
    {
        // Draw a red sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explodeRadious);
    }
}
