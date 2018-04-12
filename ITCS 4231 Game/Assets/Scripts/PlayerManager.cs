using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private Animator anim;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Transform cam;
    //private AudioClip footstep;
    //AudioSource audioSource;
    //[SerializeField] private float turnSpeed = 5.0f;
    //[SerializeField] private Transform camTarget;

    // Use this for initialization
    void Start() {
        //audioSource = GetComponent<AudioSource>();
        anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.idle);
        anim.SetInteger(HashIDs.self.playerStateInt, (int)PlayerState.standing);
    }

    // Update is called once per frame
    void Update() {

        //print("Movement type = " + HashIDs.self.playerMovementTypeInt);
        // TODO set forward running direction equal to direction camera is facing

        float xmove = Input.GetAxis("Horizontal");
        float zmove = Input.GetAxis("Vertical");

        //print("xmove = " + xmove);
        //print("zmove = " + zmove);
        //print("state " + HashIDs.self.playerMovementTypeInt);
        

        if(xmove == 0 && zmove == 0)        //Player is not moving
        {
            // Set moveType to 0 to stay in idle animation
            anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.idle);
            //print("Movement: " + playerMovementTypeInt.idle);
        }
        if (zmove > 0)      //Moving forward
        {
            anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.forward);
        }
        else if(zmove < 0)       //Moving backwards
        {
            anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.backward);
        }
        else if (xmove > 0)
        {
            anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.right);
        }
        else if (xmove < 0)
        {
            anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.left);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            print("State int : " + anim.GetInteger(HashIDs.self.playerStateInt));
            if (anim.GetInteger(HashIDs.self.playerStateInt) == 0) {
                print("time to crouch!");
                anim.SetInteger(HashIDs.self.playerStateInt, (int)PlayerState.crouched);
            }
            else if (anim.GetInteger(HashIDs.self.playerStateInt) == 1) {
                print("Standing up!");
                anim.SetInteger(HashIDs.self.playerStateInt, (int)PlayerState.standing);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.dive);
        }

    }

   /* void playFootStep()
    {
//<<<<<<< HEAD
        //audioSource = footstep[Random.Range(0, footstep.Length()];
//=======
        //audioSource = footstep[Random.Range(0, footstep.length);
//>>>>>>> 55916f5deba8b87ec7fad2ba2831f4928f46e12c
        audioSource.volume = 0.3f;
        audioSource.Play();
    }*/

    public void SetDirection()
    {
        //Vector3 towards = new Vector3(0f, cam.rotation.y, 0f).normalized;
        //towards = Quaternion.Euler(towards.x, towards.y, towards.z);
        //Debug.DrawRay(transform.position + Vector3.up, towards, Color.white);
        //if (transform.rotation.y != cam.rotation.y)
        //    transform.Rotate(towards); // Quaternion.Euler(towards.x, towards.y, towards.z);
        //transform.rotation = Quaternion.LookRotation(towards, Vector3.up);

        Vector3 towards = transform.eulerAngles;
        towards.y = Camera.main.transform.eulerAngles.y;
        transform.eulerAngles = towards;
    }
}
