using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMeshrenderer : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
