using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE: it inherits from the base animal
public class Snake : Animal
{
    // POLYMORPHISM: overriding the base start method, to set variables to values specific to this animal
    protected override void Start()
    {
        base.Start();
        jumpHeight = 1;
        runSpeed = 5;
        turnSpeed = 100;

        characterPos = new();
        camPos = new();
        camDir = new();
    }
}
