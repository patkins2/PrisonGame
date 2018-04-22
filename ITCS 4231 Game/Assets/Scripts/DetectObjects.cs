using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjects : MonoBehaviour
{

    [SerializeField] private float openDist = 3;
    private float dist = 0;
    [SerializeField] private Animator doorAnim;
    [SerializeField] private Transform player;
    [SerializeField] private Transform door;
    public GameObject detectedObject;
    public string detected = "";


    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, (transform.position - Camera.main.transform.position), Color.red);

        if (Physics.Raycast(transform.position, (transform.position - Camera.main.transform.position), out hit))
        {
            if (hit.collider.tag == "Door" && hit.distance < openDist)
            {
                dist = hit.distance;
                detected = "door";
                detectedObject = hit.collider.gameObject;
            }
            else
            {
                dist = 0f;
                detected = "";
            }
            //TODO check for item detected
        }
    }
}
