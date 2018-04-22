using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private Animator anim;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject camTarget;
    //[SerializeField] private float turnSpeed = 5.0f;

    // Use this for initialization
    void Start() {
        anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.idle);
        anim.SetInteger(HashIDs.self.playerStateInt, (int)PlayerState.standing);
    }

    // Update is called once per frame
    void Update() {

        float xmove = Input.GetAxis("Horizontal");
        float zmove = Input.GetAxis("Vertical");

        //print("player" + transform.position);
        //Player is not moving
        if (xmove == 0 && zmove == 0) { anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.idle); }
        //Press W to move forward
        if (zmove > 0) { anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.forward); }
        //Press S to move backward
        if (zmove < 0) { anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.backward); }
        //Press D to move right
        if (xmove > 0) { anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.right); }
        //Press A to move left
        if (xmove < 0) { anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.left); }
        //Press spacebar to roll forward

        if (Input.GetKeyDown(KeyCode.Space)) { anim.SetInteger(HashIDs.self.playerMovementTypeInt, (int)PlayerMovementType.dive); }
        //Press C to toggle crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (anim.GetInteger(HashIDs.self.playerStateInt) == 0) {
                anim.SetInteger(HashIDs.self.playerStateInt, (int)PlayerState.crouched);
            }
            else if (anim.GetInteger(HashIDs.self.playerStateInt) == 1) {
                anim.SetInteger(HashIDs.self.playerStateInt, (int)PlayerState.standing);
            }
        }

        //Press tab to switch camera shoulder
        if(Input.GetKeyDown(KeyCode.Tab))
        { 
            if (camTarget.transform.localPosition.x > 0f)
                camTarget.transform.Translate(-(0.25f * 2), 0f, 0f);
            else if (camTarget.transform.localPosition.x < 0f)
                camTarget.transform.Translate((0.25f * 2), 0f, 0f);
        }

        //TODO check which object is detected (door or item) and call appropriate function
        //Press F to interact with door
        if (Input.GetKeyDown(KeyCode.F) && camTarget.GetComponent<DetectObjects>().detected == "door")
        {
            
            GameObject door = camTarget.GetComponent<DetectObjects>().detectedObject;
            door.GetComponent<DoorController>().OpenClose();
            print("ITS DOOR TIME");
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
        Vector3 towards = transform.eulerAngles;
        towards.y = Camera.main.transform.eulerAngles.y;
        transform.eulerAngles = towards;
    }
}
