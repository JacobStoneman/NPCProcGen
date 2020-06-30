using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleScript : MonoBehaviour
{
    public GameObject ObjectToToggle;
    public bool TurnedOn = false;
    NPCController npcCon;

    private void Start()
    {
        ForceOff();
        npcCon = GameObject.Find("Controller").GetComponent<NPCController>();
        ObjectToToggle.SetActive(TurnedOn);
    }

    private void Update()
    {
        if(npcCon.intTime == npcCon.nightStart)
        {
            ForceOn();
        } else if(npcCon.intTime == npcCon.nightEnd)
        {
            ForceOff();
        }
    }

    public void Toggle()
    {
        if (TurnedOn)
        {
            TurnedOn = false;
        }
        else
        {
            TurnedOn = true;
        }

        ObjectToToggle.SetActive(TurnedOn);
    }

    public void ForceOn()
    {
        ObjectToToggle.SetActive(true);
        TurnedOn = true;
    }

    public void ForceOff()
    {
        ObjectToToggle.SetActive(false);
        TurnedOn = false;
    }
}
