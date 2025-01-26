using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlasmaLauncher : NetworkBehaviour
{

    public bool enableByDefault = false;


    public GameObject plasmaPrefab;
    public Camera cam;
    public float launchDistanceFromCam = 0.5f;
    public float plazmaFlySpeed = 5.0f;
    public Animator animator;

    public override void OnStartClient()
    {
        this.enabled = false;
        if (isLocalPlayer)
            enablePlazmaGun(enableByDefault);
    }


    public void enablePlazmaGun(bool isEnable)
    {
        this.enabled = isEnable;
        animator.SetBool("IsHoldingRifle", isEnable);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) { 
            if (isLocalPlayer)
            {
                cmdLaunchPlazma(cam.transform.position + cam.transform.forward * launchDistanceFromCam,
                    cam.transform.rotation);
                animator.SetTrigger("Fire");
            }
        }
    }

    [Command]
    public void cmdLaunchPlazma(Vector3 pos, Quaternion rot)
    {
        GameObject plazma = Instantiate(plasmaPrefab, pos, rot);
        NetworkServer.Spawn(plazma);
        plazma.GetComponent<Rigidbody>().AddForce(plazma.transform.forward * plazmaFlySpeed, ForceMode.VelocityChange);
    }
}
