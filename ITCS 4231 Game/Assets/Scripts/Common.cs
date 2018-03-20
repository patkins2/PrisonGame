using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType {
    idle = 0 ,
    forward = 1,
    backward = 2,
    left = 3,
    right = 4,
    dive = 5,
    interact = 6
}

public enum State {
    standing = 0,
    crouched = 1,
    action = 2
}

