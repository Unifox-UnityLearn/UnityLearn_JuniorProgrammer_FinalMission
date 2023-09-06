using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE: it inherits from the base animal
public class Gecko : Animal
{
    protected override void Start()
    {
        base.Start();
        jumpHeight = 15;
        runSpeed = 5;
        turnSpeed = 30;

        characterPos = new();
        camPos = new();
        camDir = new();
    }
}
