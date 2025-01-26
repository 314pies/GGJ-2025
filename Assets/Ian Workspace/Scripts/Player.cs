using ECM.Components;
using ECM.Controllers;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{

    // Enable these if it's local player
    [Header("Local Player Components")]
    public CharacterMovement characterMovement;
    public BaseFirstPersonController baseFirstPersonController;
    public Rigidbody rigidbody;
    public Camera cam;
    public AudioListener audioListener;
    public Canvas canvas;

    [Header("Animations")]
    public Animator animator;
    public float xAnimMultiplier = 0.1f, zAnimMultiplier = 0.1f;
    public float xVel = 0, zVal = 0;
    private int xVelAnimParm = Animator.StringToHash("xVel");
    private int zVelAnimParm = Animator.StringToHash("zVel");
    private int isGroundAnimParm = Animator.StringToHash("isGround");
    // For IK
    public PlayerIK playerIK;
    public float iKSmoothTime = 0.07f;
    private Vector3 iKSmoothVelocity;

    [SyncVar(hook = nameof(onPlayerStateChanged))]
    public int playerState;

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            characterMovement.enabled = false;
            baseFirstPersonController.enabled = false;
            rigidbody.isKinematic = false;
            cam.enabled = true;
            audioListener.enabled = true;
            canvas.gameObject.SetActive(true);
        }
        else
        {
            characterMovement.enabled = false;
            baseFirstPersonController.enabled = false;
            rigidbody.isKinematic = true;
            cam.enabled = false;
            audioListener.enabled = false;
            canvas.gameObject.SetActive(false);
        }
    }

    void setupLocalPlayer()
    {
        characterMovement.enabled = true;
        baseFirstPersonController.enabled = true;
        rigidbody.isKinematic = false;
        cam.enabled = true;
        audioListener.enabled = true;
        
    }


    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            switch ((PlayerState) playerState)
            {
                case PlayerState.ALIVE:
                    handleAliveUpdate();
                    break;
                case PlayerState.SPECTATING:
                case PlayerState.DEAD:
                    //handleSpectatingUpdate();
                    break;
            }
        }

        if (isServer && transform.position.y < -1 && (PlayerState) playerState == PlayerState.ALIVE)
        {
            syncPlayerStateChage(PlayerState.SPECTATING);
        }

        playerIK.lookAt = Vector3.SmoothDamp(playerIK.lookAt, latestLookAtPos, ref iKSmoothVelocity, iKSmoothTime);
    }

    //[Command(channel = Channels.Reliable)]
    public void syncPlayerStateChage(PlayerState givenPlayerState)
    {
        playerState = (int) givenPlayerState;
        GameObject.FindGameObjectWithTag("GameStateManager")
            .GetComponent<GameStateManager>()
            .ServerOnClientFallEvent(gameObject);
    }

    void onPlayerStateChanged(int oldState, int newState) {
        if ((PlayerState) oldState != PlayerState.ALIVE)
        {
            return;
        }

        switch ((PlayerState) newState)
        {
            case PlayerState.SPECTATING:
                enableGlobalCamera();
                break;
            default:
                return;
        }
    }

    void enableGlobalCamera()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        cam.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GameObject globalCamera = GameObject.FindWithTag("GlobalCamera");
        globalCamera.GetComponent<Camera>().enabled = true;
        globalCamera.GetComponent<FlyCamera>().enabled = true;
    }

    void disableGlobalCamera()
    {
        cam.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GameObject globalCamera = GameObject.FindWithTag("GlobalCamera");
        globalCamera.GetComponent<Camera>().enabled = false;
        globalCamera.GetComponent<FlyCamera>().enabled = false;
    }

    void handleSpectatingUpdate()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rigidbody.velocity);
        xVel = localVelocity.x * xAnimMultiplier;
        zVal = localVelocity.z * zAnimMultiplier;
        animator.SetFloat(xVelAnimParm, xVel);
        animator.SetFloat(zVelAnimParm, zVal);
        animator.SetBool(isGroundAnimParm, false);
    }

    void handleAliveUpdate()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rigidbody.velocity);
        xVel = localVelocity.x * xAnimMultiplier;
        zVal = localVelocity.z * zAnimMultiplier;
        animator.SetFloat(xVelAnimParm, xVel);
        animator.SetFloat(zVelAnimParm, zVal);
        animator.SetBool(isGroundAnimParm, GetComponent<GroundDetection>().isOnGround);
        if (Input.GetKeyDown(KeyCode.L))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 300, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            CmdSyncLookAt(cam.transform.position + cam.transform.forward * 100);
        }
    }

    [SyncVar]
    public Vector3 latestLookAtPos;

    // Command method
    [Command(channel = Channels.Unreliable)]
    public void CmdSyncLookAt(Vector3 latestPos)
    {
        latestLookAtPos = latestPos;
    }

    [SerializeField]
    private Vector3[] spawnVectors =
    {
        new Vector3(0, 150, 0)
    };

    public float setSpawnPointTime = 0.6f;
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        if (!isLocalPlayer) return;

        StartCoroutine(waitForSpawn(setSpawnPointTime));
    }

    IEnumerator waitForSpawn(float delay)
    {
        // Wait for the specified time
        yield return new WaitForSeconds(delay);

        Vector3 spawnPoint = spawnVectors[Random.Range(0, spawnVectors.Length)];
        transform.position = spawnPoint;
        setupLocalPlayer();
    }

    public Animator cameraAnimator;
    public void playCameraShakeEffect()
    {
        cameraAnimator.SetTrigger("CameraShakeTrigger");
    }

    public enum PlayerState
    {
        ALIVE,
        DEAD,
        SPECTATING,
    }
}
