using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class HashIDs : MonoBehaviour {
 
    public static HashIDs self;
 
    public int movementTypeInt;
 
    void Awake () {
 
        // The singleton reference hasn't been set yet -> set it
        if (self == null) {
            self = this;
            DontDestroyOnLoad (gameObject);
        } 
        // The singleton already exists -> destroy this one
        else {
             
            Destroy (gameObject);
        }
 
    }
 
    void Start () {
        movementTypeInt = Animator.StringToHash ("movementType");
    }
 
}