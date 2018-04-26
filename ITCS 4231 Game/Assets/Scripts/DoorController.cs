using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    [SerializeField] private Transform player;
    [SerializeField] private Animator doorAnim;
    private string hinge = "";
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        doorAnim = GetComponent<Animator>();
        hinge = doorAnim.runtimeAnimatorController.name;
    }

    public void OpenClose()
    {
        /*
        if (doorAnim.GetBool("Open") == false)
        {
            print("gonna open door");
            if (player.position.z < transform.position.z)
            {
                print("player less than door");
                doorAnim.SetBool("direction", false);
            }
            else
            {
                print("player more than door");
                doorAnim.SetBool("direction", true);
            }
            doorAnim.SetBool("open", true);
            print("door is open?");
        }
        else
        {
            print("closing door");
            doorAnim.SetBool("open", false);
        }
        */

        //check if door is opened or closed
        if (doorAnim.GetBool("Open") == false)
        {
            //check which side of door player is on
            if (hinge == "DoorRightHinge")
            {
                if (player.position.z > transform.position.z)
                    doorAnim.SetBool("Direction", true);
                else
                    doorAnim.SetBool("Direction", false);
            }
            else
            {
                if (player.position.z < transform.position.z)
                    doorAnim.SetBool("Direction", true);
                else
                    doorAnim.SetBool("Direction", false);
            }

            print(hinge);
            if (hinge == "DoorRightHinge")
            {
                print("righty + " + doorAnim.GetBool("Direction"));
                if (doorAnim.GetBool("Direction") == true)
                {
                    print("O1");
                    doorAnim.Play("DoorOpen1");
                }
                else
                {
                    print("O2");
                    doorAnim.Play("DoorOpen2");
                }
            }
            else
            {
                print("lefty + " + doorAnim.GetBool("Direction"));
                if (doorAnim.GetBool("Direction") == true)
                {
                    print("O1");
                    doorAnim.Play("DoorOpen1");
                }
                else
                {
                    print("O2");
                    doorAnim.Play("DoorOpen2");
                }
            }

            doorAnim.SetBool("Open", true);
        }
        else
        {
            if (hinge == "DoorRightHinge")
            { 
                if (doorAnim.GetBool("Direction"))
                    doorAnim.Play("DoorClose1");
                else
                    doorAnim.Play("DoorClose2");
                doorAnim.SetBool("Open", false);
            }
            else
            {
                if (doorAnim.GetBool("Direction"))
                    doorAnim.Play("DoorClose1");
                else
                    doorAnim.Play("DoorClose2");
                doorAnim.SetBool("Open", false);
            }
        }
            
    }
}
