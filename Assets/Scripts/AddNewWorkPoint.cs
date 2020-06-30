using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddNewWorkPoint : MonoBehaviour
{
    NPCController npcCon;
    public GameObject point;
    public string description;
    public string title;

    public float exLow;
    public float exHigh;

    public float agLow;
    public float agHigh;

    public float conLow;
    public float conHigh;

    public float neuroLow;
    public float neuroHigh;

    public float openLow;
    public float openHigh;

    void Start()
    {
        npcCon = GameObject.Find("Controller").GetComponent<NPCController>();

        NPCController.WorkPoint newWorkPoint = new NPCController.WorkPoint();
        newWorkPoint.point = point;
        newWorkPoint.description = description;
        newWorkPoint.title = title;
        newWorkPoint.exLow = exLow;
        newWorkPoint.exHigh = exHigh;
        newWorkPoint.agLow = agLow;
        newWorkPoint.agHigh = agHigh;
        newWorkPoint.conLow = conLow;
        newWorkPoint.conHigh = conHigh;
        newWorkPoint.neuroLow = neuroLow;
        newWorkPoint.neuroHigh = neuroHigh;
        newWorkPoint.openLow = openLow;
        newWorkPoint.openHigh = openHigh;
        npcCon.workPoints.Add(newWorkPoint);
    }
}
