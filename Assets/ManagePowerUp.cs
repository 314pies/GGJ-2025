using ECM.Components;
using ECM.Controllers;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePowerUp : NetworkBehaviour
{
    private BaseFirstPersonController bfpc;
    private CharacterMovement cm;
    private BubblePopper bubblePopper;

    [SyncVar(hook = nameof(SetSpeedPower))]
    [SerializeField] public bool speedPower = false;

    [SyncVar(hook = nameof(SetPopPower))]
    [SerializeField] public bool popPower = false;

    [SerializeField] private float maxSpeed = 20f;
    private float baseSpeed;
    private float boostedSpeed;

    
    // Start is called before the first frame update
    void Start()
    {
        bfpc = gameObject.GetComponent<BaseFirstPersonController>();
        bubblePopper = gameObject.GetComponent <BubblePopper>();
        cm = gameObject.GetComponent<CharacterMovement>();
        baseSpeed = bfpc.speed;
        boostedSpeed = bfpc.speed * 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPopPower(bool oldPower, bool newPower)
    {
        bubblePopper.power = newPower;
    }

    void SetSpeedPower(bool oldPower, bool newPower)
    {
        speedPower = newPower;

        if (speedPower)
        {
            SpeedChange(boostedSpeed, maxSpeed);
        }
        else
        {
            SpeedChange(baseSpeed);
        }
    }

    void SpeedChange(float speed, float maxSpeed = 10)
    {
        cm.maxLateralSpeed = maxSpeed;
        bfpc.speed = speed;
        bfpc.forwardSpeed = speed;
        bfpc.backwardSpeed = speed;
    }
}
