using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMovementType {
    idle = 0 ,
    forward = 1,
    backward = 2,
    left = 3,
    right = 4,
    dive = 5,
    interact = 6
};

public enum PlayerState {
    standing = 0,
    crouched = 1,
    action = 2,
    dead = 3
};

public enum GuardMovementType {
    idle = 0,
    patrol = 1,
    chase = 2,
    shoot = 3,
};

public enum GuardState {

};

