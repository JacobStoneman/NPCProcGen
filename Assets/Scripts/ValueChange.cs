using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueChange : MonoBehaviour
{
    NPCController con;
    NPCController.TraitAspect aspect;
    private void Start()
    {
        con = GameObject.Find("Controller").GetComponent<NPCController>();
    }
    public void ChangeAspect()
    {
        if (con.gameStarted)
        {
            print("Value has been changed");
        }
    }
}
