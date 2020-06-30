using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernTableController : MonoBehaviour
{
    public bool pointAvailable;
    public int maxPop;
    public int population;
    public List<GameObject> chairPoints;
    public GameObject tableParent;

    private void Awake()
    {
        maxPop = chairPoints.Count;
    }
}
