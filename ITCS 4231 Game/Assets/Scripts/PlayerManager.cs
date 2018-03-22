using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private Animator anim;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Transform cam;
    private AudioClip footstep;
    AudioSource audioSource;
    //[SerializeField] private float turnSpeed = 5.0f;
    //[SerializeField] private Transform camTarget;

    // Use this for initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        // TODO set forward running direction equal to direction camera is facing

        float xmove = Input.GetAxis("Horizontal");
        float zmove = Input.GetAxis("Vertical");

        //print("xmove = " + xmove);
        //print("zmove = " + zmove);
        //print("Movement forward : " + (int)MovementType.idle);
        //print("movement int : " + HashIDs.self.movementTypeInt);

        if(xmove == 0 && zmove == 0)
        {
            // Set moveType to 0 to stay in idle animation
            anim.SetInteger(HashIDs.self.movementTypeInt, (int)MovementType.idle);
            print("Movement: " + MovementType.idle);
        }
        if (zmove > 0)
        {
            anim.SetInteger(HashIDs.self.movementTypeInt, (int)MovementType.forward);
            print("Movement: " + MovementType.forward);
            playFootStep();
        }

        SetDirection();
    }

    void playFootStep()
    {
        //audioSource = footstep[Random.Range(0, footstep.Length()];
        audioSource.volume = 0.3f;
        audioSource.Play();
    }

    void SetDirection()
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
