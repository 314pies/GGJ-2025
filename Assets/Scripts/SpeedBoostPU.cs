using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPU : NetworkBehaviour
{
    [SerializeField] private float timer = 3.0f;
    [SerializeField] private GameObject player;
    private ManagePowerUp mpu;
    private Coroutine coroutine;

    [SyncVar(hook = nameof(changeActiveState))]
    private bool active = true;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator countdown()
    {
        yield return new WaitForSeconds(timer);
        Debug.Log("time is up");
        mpu.speedPower = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) { return; }

        if (other.tag == "Player")
        {
            player = other.gameObject;
            mpu = player.GetComponent<ManagePowerUp>();
            if (mpu != null)
            {
                mpu.speedPower = true;
                active = false;

                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(countdown());
                Debug.Log("speed boost");
            }
        }
    }

    private void changeActiveState(bool oldVal, bool newVal)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = newVal;
        gameObject.GetComponent<Collider>().enabled = newVal;
    }

}
