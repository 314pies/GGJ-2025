using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlasmaLauncher : NetworkBehaviour
{

    public bool enableByDefault = false;

    [SyncVar(hook = nameof(plazmaGunEnableStateHook))]
    public bool isPlazmaGunEnabled;
    public void plazmaGunEnableStateHook(bool oldState, bool newState)
    {
        if (isLocalPlayer)
        {
            animator.SetBool("IsHoldingRifle", newState);
            ammoText.gameObject.SetActive(newState);
        }

        plasmaRifleModel.gameObject.SetActive(newState);
    }

    [SyncVar(hook = nameof(onAmmoUpdate))]
    public int ammoCount = 5;

    public void onAmmoUpdate(int oldAmmo, int newAmmo)
    {
        if (isLocalPlayer)
        {
            ammoText.text = "" + newAmmo;
        }
    }

    public GameObject plasmaPrefab;
    public Camera cam;
    public float launchDistanceFromCam = 0.5f;
    public float plazmaFlySpeed = 5.0f;
    public Animator animator;
    public PlasmaRifleModel plasmaRifleModel;

    public TMP_Text ammoText;

    public void Awake()
    {
        plasmaRifleModel = GetComponentInChildren<PlasmaRifleModel>();
    }

    public override void OnStartClient()
    {
        
        if (isLocalPlayer)
        {
            StartCoroutine(waitAndRunDefaultEquip());
            return;
        }
    }

    IEnumerator waitAndRunDefaultEquip()
    {
        yield return new WaitForSeconds(2);
        CmdUpdateplasmaRifleEquipState(enableByDefault);
    }

    [Command]
    void CmdUpdateplasmaRifleEquipState(bool enableState)
    {
        isPlazmaGunEnabled = enableState;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (isPlazmaGunEnabled == false)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            cmdLaunchPlazma(cam.transform.position + cam.transform.forward * launchDistanceFromCam,
                cam.transform.rotation);
            animator.SetTrigger("Fire");
            GetComponent<Player>().playCameraShakeEffect();
        }

    }

    [Command]
    public void cmdLaunchPlazma(Vector3 pos, Quaternion rot)
    {
        GameObject plazma = Instantiate(plasmaPrefab, pos, rot);
        NetworkServer.Spawn(plazma);
        plazma.GetComponent<Rigidbody>().AddForce(plazma.transform.forward * plazmaFlySpeed, ForceMode.VelocityChange);
        playMuzzleEffect();
        
        ammoCount--;
        if (ammoCount <=0)
        {
            isPlazmaGunEnabled = false;
        }

    }

    [ClientRpc]
    public void playMuzzleEffect()
    {
        plasmaRifleModel.PlayMuzzleFlash();
    }
}
