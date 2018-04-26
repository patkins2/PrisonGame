using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectObjects : MonoBehaviour
{

    [SerializeField] private float interactDist = 3;

    [SerializeField] private Text interactionText;
    private string interact = "Press F to interact";
    [HideInInspector] public GameObject detectedObject;
    [HideInInspector] public string detected = "";
    private float dist = 0;

    private void Start()
    {
        interactionText.text = "";
    }

    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, (transform.position - Camera.main.transform.position), Color.red);

        if (Physics.Raycast(transform.position, (transform.position - Camera.main.transform.position), out hit))
        {
            if (hit.collider.tag == "Door" && hit.distance < interactDist)
            {
                dist = hit.distance;
                detected = "door";
                detectedObject = hit.collider.gameObject;
                interactionText.text = "Press F to open door";
            }
            else if ((hit.collider.tag == "Key Item" && hit.distance < interactDist)) 
            {
                dist = hit.distance;
                detected = "item";
                detectedObject = hit.collider.gameObject;
                interactionText.text = "Press F to pick up item";
            }
            else
            {
                dist = 0f;
                detected = "";
                interactionText.text = "";
            }
            //TODO check for item detected
        }
    }
}
