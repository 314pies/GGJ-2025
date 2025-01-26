using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoplessPU : NetworkBehaviour
{
    [SerializeField] private float timer = 3.0f;
    [SerializeField] private GameObject player;
    private ManagePowerUp mpu;
    private Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator countdown()
    {
        yield return new WaitForSeconds(timer);
        Debug.Log("time is up for pop");
        mpu.popPower = false;
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
                mpu.popPower = true;
                gameObject.GetComponent<Renderer>().enabled = false;
                gameObject.GetComponent<Collider>().enabled = false;
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(countdown());
                Debug.Log("popless");
            }
        }
    }
}
