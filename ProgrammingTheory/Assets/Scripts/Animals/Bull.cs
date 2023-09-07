using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE: it inherits from the base animal
public class Bull : Animal
{
    // POLYMORPHISM: overriding the base start method, to set variables to values specific to this animal
    protected override void Start()
    {
        base.Start();
        jumpHeight = 5;
        runSpeed = 30;
        turnSpeed = 5;

        camPos = new(0, 1.5f, 1.5f);
        characterPos = new(0, 1.5f, 1.5f);
        camDir = new(40, 180, 0);
    }
}
