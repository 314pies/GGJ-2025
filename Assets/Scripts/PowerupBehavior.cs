using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerupBehavior : NetworkBehaviour
{
    private List<GameObject> floor = new List<GameObject>();
    [SerializeField] private GameObject powerup;
    [SerializeField] private float delay = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor")) 
        {
            floor.Add(obj);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isServer) { return; }
        if (delay < 0)
        {
            // remove destroyed bubbles, select random bubble, and place powerup on unoccupied bubble
            floor.RemoveAll(x => x == null);
            GameObject bubble = floor[Random.Range(0, floor.Count - 1)];
            Vector3 loc = bubble.transform.localPosition;
            loc.y++;
            bool sameLoc = false;
            Collider[] colliders = Physics.OverlapSphere(loc, bubble.transform.localScale.x);
            foreach (Collider collider in colliders)
            {
                if (collider.name == "Powerup(Clone)")
                {
                    Debug.Log("detected powerup on same bubble");
                    sameLoc = true;
                    break;
                }
            }
            if (!sameLoc)
            {
                GameObject pu = Instantiate(powerup, loc, Quaternion.identity);
                NetworkServer.Spawn(pu);
                
            }
            
            delay = 3f;
            Debug.Log("reset timer");
        }
        delay -= Time.deltaTime;
    }
}
