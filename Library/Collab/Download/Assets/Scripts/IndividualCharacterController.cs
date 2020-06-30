using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndividualCharacterController : MonoBehaviour
{
    public GameObject camTarget;
    public GameObject speechCanvas;
    public Image mean;
    public Image nice;

    private void Awake()
    {
        speechCanvas.SetActive(false);
    }

    public void SetImage(int image)
    {
        speechCanvas.SetActive(true);
        if(image == 0)
        {
            nice.enabled = true;
            mean.enabled = false;
        } else if(image == 1)
        {
            nice.enabled = false;
            mean.enabled = true;
        }
    }
}
