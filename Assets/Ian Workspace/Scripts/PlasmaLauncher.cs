using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlasmaLauncher : NetworkBehaviour
{

    public bool enableByDefault = false;

    [SyncVar(hook = nameof(plazmaGunEnableStateHook))]
    public bool isPlazmaGunEnabled;

    public static int IsHoldingRifleAnimParm { get; private set; } = Animator.StringToHash("IsHoldingRifle");
    Character character { get { return GetComponent<Player>().currentCharacter; } }
    GameStateManager gameStateManager { get {  return GetComponent<Player>().gameStateManager; } }

    public void plazmaGunEnableStateHook(bool oldState, bool newState)
    {
        if (isLocalPlayer)
        {
            character.animator.SetBool(IsHoldingRifleAnimParm, newState);
            ammoText.gameObject.SetActive(newState);
        }
        character.UpdateStatus(newState);
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

    public TMP_Text ammoText;

    public AudioClip fireSoundEffect;
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
            CmdLaunchPlazma(cam.transform.position + cam.transform.forward * launchDistanceFromCam,
                cam.transform.rotation);
            character.animator.SetTrigger("Fire");
            GetComponent<Player>().playCameraShakeEffect();
            GetComponent<AudioSource>().PlayOneShot(fireSoundEffect, 0.5f);
        }
    }

    [Command]
    public void CmdLaunchPlazma(Vector3 pos, Quaternion rot)
    {
        GameObject plazma = Instantiate(plasmaPrefab, pos, rot);
        NetworkServer.Spawn(plazma);
        plazma.GetComponent<Rigidbody>().AddForce(plazma.transform.forward * plazmaFlySpeed, ForceMode.VelocityChange);
        RpcPlayMuzzleEffect();

        if (gameStateManager.gameState != GameStateManager.GameState.Wait)
        {
            ammoCount--;
        }

        if (ammoCount <= 0)
        {
            isPlazmaGunEnabled = false;
        }
    }

    [ClientRpc]
    public void RpcPlayMuzzleEffect()
    {
        character.plasmaRifleModel.PlayMuzzleFlash();
    }
}
