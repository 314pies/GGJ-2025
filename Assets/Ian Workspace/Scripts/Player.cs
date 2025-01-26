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
        }
        else
        {
            characterMovement.enabled = false;
            baseFirstPersonController.enabled = false;
            rigidbody.isKinematic = true;
            cam.enabled = false;
            audioListener.enabled = false;
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

    private Vector3[] spawnVectors =
    {
        new Vector3(0, 90, 0)
    };

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        if (!isLocalPlayer) return;

        StartCoroutine(waitForSpawn(2));
    }

    IEnumerator waitForSpawn(float delay)
    {
        // Wait for the specified time
        yield return new WaitForSeconds(delay);

        Vector3 spawnPoint = spawnVectors[Random.Range(0, spawnVectors.Length)];
        transform.position = spawnPoint;
        setupLocalPlayer();
    }
}
