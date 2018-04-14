using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class HashIDs : MonoBehaviour {
 
    public static HashIDs self;
 
    public int playerMovementTypeInt;
    public int guardMovementTypeInt;
    public int playerStateInt;
 
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

        playerMovementTypeInt = Animator.StringToHash("playerMovementType");
        playerStateInt = Animator.StringToHash("playerState");

        guardMovementTypeInt = Animator.StringToHash("guardMovementType");
    }
 
}