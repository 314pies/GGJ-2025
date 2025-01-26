using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazmagunToPickup : NetworkBehaviour
{
    public int fullAmmoCount = 5;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" 
              && collision.gameObject.GetComponent<Player>().isLocalPlayer)
        {
            CmdPickUp(collision.gameObject.GetComponent<NetworkIdentity>());
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdPickUp(NetworkIdentity identity)
    {
        identity.gameObject.GetComponent<PlasmaLauncher>().isPlazmaGunEnabled = true;
        identity.gameObject.GetComponent<PlasmaLauncher>().ammoCount = fullAmmoCount;
        Destroy(gameObject);
    }

    public float rotationSpeed = 15f; // Degrees per second
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
