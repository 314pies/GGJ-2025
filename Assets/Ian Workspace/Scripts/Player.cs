using ECM.Components;
using ECM.Controllers;
using Mirror;
using System.Collections;
using System.Collections.Generic;
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
    

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            characterMovement.enabled = true;
            baseFirstPersonController.enabled = true;
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

            //animator.SetTrigger("Jump");

        }
    }
}
