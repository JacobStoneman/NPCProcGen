using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernController : MonoBehaviour
{
    public List<GameObject> tavernPoints;
    public List<GameObject> tables;
    NPCController npcCon;

    private void Awake()
    {
        npcCon = GameObject.Find("Controller").GetComponent<NPCController>();
        npcCon.CreateTavern(tavernPoints,tables);
    }
}
