using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectUIController : MonoBehaviour
{
    public Slider slider;
    public Text left;
    public Text right;
    public int id;
    NPCController con;
    private void Start()
    {
        con = GameObject.Find("Controller").GetComponent<NPCController>();
    }
    public void ChangeAspect()
    {
        if (con != null)
        {
            if (con.gameStarted)
            {              
                con.UpdateNPCValues(id, slider.value);
            }
        }
    }
}
