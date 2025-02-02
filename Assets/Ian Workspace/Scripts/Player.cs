using ECM.Components;
using ECM.Controllers;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    GameStateManager _gameStateManager;
    public GameStateManager gameStateManager
    {
        get
        {
            if (_gameStateManager == null)
            {
                _gameStateManager = GameObject.FindGameObjectWithTag("GameStateManager")
                        .GetComponent<GameStateManager>();
            }
            return _gameStateManager;
        }
    }
    // Enable these if it's local player
    [Header("Local Player Components")]
    public CharacterMovement characterMovement;
    public BaseFirstPersonController baseFirstPersonController;
    public Rigidbody rigidbody;
    public Camera cam;
    public AudioListener audioListener;
    public Canvas canvas;
    public PlasmaLauncher plasmaLauncher
    {
        get
        {
            return GetComponent<PlasmaLauncher>();
        }
    }

    [Header("Animations")]
    public float xAnimMultiplier = 0.1f, zAnimMultiplier = 0.1f;
    public float xVel = 0, zVal = 0;
    private int xVelAnimParm = Animator.StringToHash("xVel");
    private int zVelAnimParm = Animator.StringToHash("zVel");
    public static int isGroundAnimParm { get; private set; } = Animator.StringToHash("isGround");
    // For IK
    public float iKSmoothTime = 0.07f;
    private Vector3 iKSmoothVelocity;

    [SyncVar(hook = nameof(onPlayerStateChanged))]
    public PlayerState playerState;

    [Header("Character Customization")]
    public Character[] characters;
    public Character currentCharacter;
    public GameObject CharacterSelectionUI;
    [SyncVar(hook = nameof(OnChacterSelectionUpdate))]
    public int currentCharacterIndex = 0;

    [SyncVar(hook = nameof(OnEnableCharacterSelectionUpdate))]
    public bool isEnableCharacterSelection = true;

    private void OnChacterSelectionUpdate(int oldChar, int newChar)
    {
        Character oldCharacter = characters[oldChar];
        Character newCharacter = characters[newChar];

        newCharacter.gameObject.SetActive(true);
        newCharacter.Initialize(oldCharacter.animator,
            plasmaLauncher.isPlazmaGunEnabled);
        currentCharacter = newCharacter;

        GetComponent<NetworkAnimator>().animator = currentCharacter.animator;
        oldCharacter.gameObject.SetActive(false);
    }

    [Command]
    public void CmdSwitchCharacter()
    {
        if (currentCharacterIndex >= characters.Length - 1)
        {
            currentCharacterIndex = 0;
        }
        else
        {
            currentCharacterIndex++;
        }
    }

    [Command]
    public void CmdLockInCharacter()
    {
        isEnableCharacterSelection = false;
    }
    private void OnEnableCharacterSelectionUpdate(bool oldState, bool newState)
    {
        CharacterSelectionUI.SetActive(newState);
    }

    public override void OnStartClient()
    {
        initializePlayer(isLocalPlayer);
    }
    private void initializePlayer(bool enableLocalPlayer)
    {
        characterMovement.enabled = enableLocalPlayer;
        baseFirstPersonController.enabled = enableLocalPlayer;
        rigidbody.isKinematic = !enableLocalPlayer;
        cam.enabled = enableLocalPlayer;
        audioListener.enabled = enableLocalPlayer;
        canvas.gameObject.SetActive(enableLocalPlayer);
    }


    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            switch (playerState)
            {
                case PlayerState.ALIVE:
                    LocalPlayerHandleAliveUpdate();
                    break;
                case PlayerState.SPECTATING:
                    break;
                case PlayerState.DEAD:
                    break;
            }
        }

        if (isServer && transform.position.y < -1 && playerState == PlayerState.ALIVE 
            && gameStateManager.gameState == GameStateManager.GameState.InGame)
        {
            TriggerPlayerFallEvent();
        }

        currentCharacter.playerIK.lookAt = Vector3.SmoothDamp(currentCharacter.playerIK.lookAt,
            latestLookAtPos, ref iKSmoothVelocity, iKSmoothTime);
    }

    public void TriggerPlayerFallEvent()
    {
        playerState = PlayerState.SPECTATING;
        gameStateManager.ServerOnClientFallEvent(gameObject);
    }

    void onPlayerStateChanged(PlayerState oldState, PlayerState newState)
    {
        if (newState == PlayerState.SPECTATING)
        {
            enableFlyCamera();
            return;
        }
    }

    void enableFlyCamera()
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

    void LocalPlayerHandleAliveUpdate()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rigidbody.velocity);
        xVel = localVelocity.x * xAnimMultiplier;
        zVal = localVelocity.z * zAnimMultiplier;
        currentCharacter.animator.SetFloat(xVelAnimParm, xVel);
        currentCharacter.animator.SetFloat(zVelAnimParm, zVal);
        currentCharacter.animator.SetBool(isGroundAnimParm, GetComponent<GroundDetection>().isOnGround);

        if (isEnableCharacterSelection && Input.GetKeyDown(KeyCode.Q))
        {
            CmdSwitchCharacter();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            CmdLockInCharacter();
        }

        // Backdoor
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    GetComponent<Rigidbody>().AddForce(Vector3.up * 300, ForceMode.Impulse);
        //}
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
