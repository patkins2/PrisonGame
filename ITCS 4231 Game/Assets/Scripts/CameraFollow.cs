using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform trans;
    public Vector3 lookDir;
    public GameObject target;
    public float sensitivity = 5.0f;
    private float dist;
    public float radiusOfSatisfaction = 0.1f;
    public Vector3 targetPos;

	// Use this for initialization
	void Start () {
        targetPos = target.transform.position;
        dist = Vector3.Distance(trans.position, targetPos);
    }
	
	// Update is called once per frame
	void LateUpdate () {

        // TODO move camera closer to player if view is obscured, and back to default distance if player is visible

        // DONE basic camera movement
        {
            //Rotates camera to look at player
            lookDir = targetPos - trans.position;
            trans.rotation = Quaternion.LookRotation(lookDir);

            //Moves camera in orbit around player with mouse movement
            trans.RotateAround(targetPos, Vector3.up, sensitivity * Input.GetAxis("Mouse X") * Time.deltaTime);
            trans.RotateAround(targetPos, Vector3.right, sensitivity * Input.GetAxis("Mouse Y") * -1 * Time.deltaTime);

            //Moves camera with the player
            if (targetPos != target.transform.position)
            {
                trans.position += (target.transform.position - targetPos);
                targetPos = target.transform.position;
            }
        }

        // Adjust camera distance to ensure player is in view
        RaycastHit hit;

        Ray playerLOS = new Ray(trans.position, targetPos - trans.position);
        Debug.DrawRay(trans.position, lookDir, Color.red);

        Debug.DrawRay(trans.position, -trans.forward, Color.green);
        if (Physics.Raycast(playerLOS, out hit))
        {
            if (hit.collider.tag == "CameraTarget")
            {
                Debug.Log("variable dist = " + dist);
                Debug.Log("dist compared with = " + Vector3.Distance(trans.position, targetPos));
                //Debug.Log("Can see player");

                // Set Raycast firing backwards from camera to determine if cam will
                // be blocked from returning to default distance
                // GOAL: prevent camera shaking

                Ray rear = new Ray(trans.position, -trans.forward);
                
                if (dist - radiusOfSatisfaction >= Vector3.Distance(trans.position, targetPos) && !Physics.Raycast(rear, out hit, 0.2f))
                {
                    trans.position = Vector3.MoveTowards(trans.position, targetPos, -dist * Time.deltaTime);
                }
            }
            else
            {
                //Debug.Log("Can NOT see player");
                //Debug.Log("Looking at :" + hit.collider.name);
                // Moves camera towards player if view is obscured
                trans.position = Vector3.MoveTowards(trans.position, targetPos, Time.deltaTime);
            }
        }

    }
}
