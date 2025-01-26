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

    [Header("CharacterSececltion")]
    public GameObject[] Characters;
    [SyncVar(hook = nameof(onSelectedCharacterUpdate))]
    public int currectCharacter = 0;
    public GameObject characterSelectionUI;
    [SyncVar(hook = nameof(onIsCharacterSelectionEnabledUpdate))]
    public bool isCharacterSelectionEnabled = true;

    public void onSelectedCharacterUpdate(int old, int selectedChar)
    {
        foreach (var character in Characters)
        {
            character.SetActive(false);
        }
        GameObject currentPlayer = Characters[selectedChar];
        currentPlayer.SetActive(true);
        animator = currentPlayer.GetComponent<Animator>();
        playerIK = currentPlayer.GetComponent<PlayerIK>();

        GetComponent<PlasmaLauncher>().Initialize(Characters[currectCharacter], animator);        
        GetComponent<NetworkAnimator>().animator = animator;
        StartCoroutine(resetNetworkAnimatorAfter());
    }

    IEnumerator resetNetworkAnimatorAfter()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<NetworkAnimator>().Reset();
        Debug.Log("animator resetted");
    }

    public void onIsCharacterSelectionEnabledUpdate(bool old, bool isSelectionEnabled)
    {
        if (isLocalPlayer)
        {
            characterSelectionUI.SetActive(isSelectionEnabled);
        }
    }

    [Command]
    void CmdSwitchPlayer()
    {
        if (!isCharacterSelectionEnabled)
        {
            return;
        }

        if (currectCharacter < Characters.Length - 2)
        {
            currectCharacter++;
        }
        else
        {
            currectCharacter = 0;
        }
    }

    [Command]
    void CmdEnableSwitchPlayer(bool enableSwitchPlayer)
    {
        Debug.Log("CmdEnableSwitchPlayer: " + enableSwitchPlayer);
        isCharacterSelectionEnabled = enableSwitchPlayer;
    }

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
            characterSelectionUI.SetActive(false);
        }
        GetComponent<PlasmaLauncher>().Initialize(Characters[currectCharacter], animator);
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


        playerIK.lookAt = Vector3.SmoothDamp(playerIK.lookAt, latestLookAtPos, ref iKSmoothVelocity, iKSmoothTime); ;

        if (isCharacterSelectionEnabled && isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CmdSwitchPlayer();

            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log("asdasdasd");
                CmdEnableSwitchPlayer(false);
            }
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
        new Vector3(0, 90, 0)
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
}
