using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    NPCController npcCon;
    public Vector3 targetCamPosition;
    Quaternion targetCamRotation;
    public Material outline;
    int step = 5;
    public enum State
    {
        FREE,FOCUSED,HOUSE,CONTROL
    }
    public State camState;
    GameObject characterCanvas;
    GameObject freeCanvas;
    GameObject controlCanvas;
    // Start is called before the first frame update

    void Start()
    {
        Cursor.visible = false;
        npcCon = GameObject.Find("Controller").GetComponent<NPCController>();
        characterCanvas = GameObject.Find("CharacterCanvas");
        freeCanvas = GameObject.Find("FreeCanvas");
        controlCanvas = GameObject.Find("ControllerCanvas");
        characterCanvas.SetActive(false);
        camState = State.FREE;
    }
    void SetCamPos(int focus)
    {
        targetCamPosition = npcCon.characters[focus].camTarget.transform.position;
        targetCamRotation = npcCon.characters[focus].camTarget.transform.rotation;
        transform.position = Vector3.Lerp(transform.position, targetCamPosition, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetCamRotation, Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            camState = State.FREE;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            camState = State.CONTROL;
        }
        if (camState == State.FOCUSED)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            characterCanvas.SetActive(true);
            freeCanvas.SetActive(false);
            controlCanvas.SetActive(false);
            SetCamPos(npcCon.focus);
        }

        else if (camState == State.FREE)
        {
            characterCanvas.SetActive(false);
            controlCanvas.SetActive(false);
            freeCanvas.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                step = 20;
            } else
            {
                step = 5;
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.localPosition += transform.forward * step * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.localPosition -= transform.forward * step * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.localPosition -= transform.right * step * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.localPosition += transform.right * step * Time.deltaTime;
            }

            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward,out hit, Mathf.Infinity)){
                if(hit.collider.gameObject.tag == "NPC")
                {
                    hit.collider.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = outline;
                    if (Input.GetMouseButtonDown(0))
                    {
                        npcCon.setFocus(hit.collider.gameObject);
                        camState = State.FOCUSED;

                    }
                }
            }
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        else if(camState == State.HOUSE)
        {
            characterCanvas.SetActive(false);
            controlCanvas.SetActive(false);
            freeCanvas.SetActive(true);
            transform.position = targetCamPosition;
            camState = State.FREE; 
        }

        else if(camState == State.CONTROL)
        {
            characterCanvas.SetActive(false);
            freeCanvas.SetActive(false);
            controlCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
