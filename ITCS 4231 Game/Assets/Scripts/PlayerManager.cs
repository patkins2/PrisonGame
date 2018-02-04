using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public Animator anim;
    public float lookSensitivity;
    public Transform trans;
    public Quaternion camDir;

    // Use this for initialization
    void Start() {
        anim = gameObject.GetComponent<Animator>();
        trans = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {
        
        // TODO set forward running direction equal to direction camera is facing

        if (Input.anyKey == false)
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Running", false);
        }

        if (Input.GetKey("w"))
        {
            anim.SetBool("Running", true);
            anim.SetBool("Idle", false);
        }

    }
}
