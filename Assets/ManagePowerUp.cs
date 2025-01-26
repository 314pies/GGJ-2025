using ECM.Controllers;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePowerUp : NetworkBehaviour
{
    private BaseFirstPersonController bfpc;

    [SyncVar(hook = nameof(SetPower))]
    [SerializeField] public bool power = false;

    [SyncVar]
    [SerializeField] float baseSpeed;

    [SyncVar]
    [SerializeField] float boostedSpeed;

    
    // Start is called before the first frame update
    void Start()
    {
        bfpc = gameObject.GetComponent<BaseFirstPersonController>();
        baseSpeed = bfpc.speed;
        boostedSpeed = bfpc.speed * 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPower(bool oldPower, bool newPower)
    {
        Debug.Log("Changing power: " + newPower);
        power = newPower;

        if (power)
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
        bfpc.forwardSpeed = speed;
        bfpc.backwardSpeed = speed;
    }
}
