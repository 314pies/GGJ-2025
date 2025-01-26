
using Mirror;
using UnityEngine;

public class FloorsGenerator : NetworkBehaviour
{

    public GameObject floorPrefab;

    public int numberOfFloors = 4;
    public int distanceBetweenFloors = 10;
    public int distanceFromBottom = 10;

    // Start is called before the first frame update
    void Start()
    {
        if (!isServer) return;

        for (int i = 0; i < numberOfFloors; i++)
        {
            float newY = (i * distanceBetweenFloors) + distanceFromBottom;
            GameObject obj = Instantiate(floorPrefab, transform.position + new Vector3(0, newY, 0), Quaternion.identity);
            //Floor floor = obj.GetComponent<Floor>();
            NetworkServer.Spawn(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
