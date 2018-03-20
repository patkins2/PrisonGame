using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Vector3 lookDir;
    public GameObject target;
    public float sensitivity = 5.0f;
    private float dist;
    public float radiusOfSatisfaction = 0.1f;
    public float speed = 5.0f;
    public Vector3 targetPos;
    public Collider col;

	// Use this for initialization
	void Start () {
        targetPos = target.transform.position;
        dist = Vector3.Distance(transform.position, targetPos);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        // DONE basic camera movement
        {
            //Rotates camera to look at player
            lookDir = targetPos - transform.position;
            transform.rotation = Quaternion.LookRotation(lookDir);

            //Moves camera in orbit around player with mouse movement
            transform.RotateAround(targetPos, Vector3.up, sensitivity * Input.GetAxis("Mouse X") * Time.deltaTime);
            transform.RotateAround(targetPos, Vector3.right, sensitivity * Input.GetAxis("Mouse Y") * -1 * Time.deltaTime);

            //Moves camera with the player
            if (targetPos != target.transform.position)
            {
                transform.position += (target.transform.position - targetPos);
                targetPos = target.transform.position;
            }
        }

        //Move camera closer or away from player
        AdjustDistance();

    }

    void AdjustDistance() {

        // Adjust camera distance to ensure player is in view
        RaycastHit hit;
        
        Ray playerLOS = new Ray(transform.position, targetPos - transform.position);
        Debug.DrawRay(transform.position, lookDir, Color.red);

        Debug.DrawRay(transform.position + transform.forward, -transform.forward, Color.green);

        if (Physics.Raycast(playerLOS, out hit))
        {
            if (hit.collider.tag == "CameraTarget")
            {
                Debug.Log("Can see player");
                Debug.Log("Front looking at :" + hit.collider.name);

                //Debug.Log("Cam position: " + trans.position);
                //Debug.Log("Slightly in front of cam: " + (trans.position + trans.forward));

                // Set Raycast firing backwards from camera to determine if cam will
                // be blocked from

                //OLD
                //Ray rear = new Ray((trans.position+trans.forward), -trans.forward);
                //NEW
                Ray rear = new Ray(transform.position + transform.forward * 1, -transform.forward);

                if (dist - radiusOfSatisfaction >= Vector3.Distance(transform.position, targetPos) && !Physics.Raycast(rear, out hit))
                {
                    //Debug.Log("Rear looking at :" + hit.collider.name);

                    transform.position = Vector3.MoveTowards(transform.position, targetPos, -dist * Time.deltaTime * speed);
                }
            }
            else
            {
                Debug.Log("Can NOT see player");
                Debug.Log("Looking at :" + hit.collider.name);
                // Moves camera towards player if view is obscured
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
            }
        }
    }
}
