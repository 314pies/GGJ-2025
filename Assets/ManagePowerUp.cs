using ECM.Controllers;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePowerUp : NetworkBehaviour
{
    private BaseFirstPersonController bfpc;
    private BubblePopper bubblePopper;

    [SyncVar(hook = nameof(SetSpeedPower))]
    [SerializeField] public bool speedPower = false;

    [SyncVar(hook = nameof(SetPopPower))]
    [SerializeField] public bool popPower = false;

    [SerializeField] float baseSpeed;

    [SerializeField] float boostedSpeed;

    
    // Start is called before the first frame update
    void Start()
    {
        bfpc = gameObject.GetComponent<BaseFirstPersonController>();
        bubblePopper = gameObject.GetComponent <BubblePopper>();
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
            SpeedChange(boostedSpeed);
        }
        else
        {
            SpeedChange(baseSpeed);
        }
    }

    void SpeedChange(float speed)
    {
        bfpc.speed = speed;
        bfpc.forwardSpeed = speed;
        bfpc.backwardSpeed = speed;
    }
}
