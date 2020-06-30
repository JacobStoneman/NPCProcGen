using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public GameObject bedPoint;
    public GameObject chairPoint;
    NPCController npcCon;
    Vector3 rayPosition;
    public List<GameObject> nearbyBuildings;

    private void Awake()
    {
        npcCon = GameObject.Find("Controller").GetComponent<NPCController>();
        npcCon.CreateHouse(gameObject,bedPoint, chairPoint);
        rayPosition = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!nearbyBuildings.Contains(other.gameObject))
        {
            if (other.tag == "Building")
            {
                nearbyBuildings.Add(other.gameObject);
            }
        }
    }
}
