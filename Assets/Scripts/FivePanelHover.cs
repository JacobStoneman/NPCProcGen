using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FivePanelHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text text;
    GameObject pan1;
    GameObject pan2;
    GameObject pan3;
    GameObject pan4;
    GameObject pan5;
    bool hovered;
    private void Start()
    {
        pan1 = GameObject.Find("BigFivePanel1");
        pan2 = GameObject.Find("BigFivePanel2");
        pan3 = GameObject.Find("BigFivePanel3");
        pan4 = GameObject.Find("BigFivePanel4");
        pan5 = GameObject.Find("BigFivePanel5");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && hovered)
        {
            pan1.SetActive(true);
            pan2.SetActive(true);
            pan3.SetActive(true);
            pan4.SetActive(true);
            pan5.SetActive(true);
            gameObject.SetActive(false);
            hovered = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //GetComponent<Image>().color = new Color(255, 255, 255, 0);
        //text.enabled = false;
        hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //GetComponent<Image>().color = new Color(255, 255, 255, 255);
        //text.enabled = true;
        hovered = false;
    }
}
