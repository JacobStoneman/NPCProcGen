using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackstoryDisplay : MonoBehaviour
{
    public void SetText(string _text)
    {
        GetComponentInChildren<Text>().text = _text;
    }
}
