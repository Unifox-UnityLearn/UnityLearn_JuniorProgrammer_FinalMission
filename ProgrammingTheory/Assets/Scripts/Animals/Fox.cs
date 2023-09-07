using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE: it inherits from the base animal
public class Fox : Animal
{
    // POLYMORPHISM: overriding the base start method, to set variables to values specific to this animal
    protected override void Start()
    {
        base.Start();
        jumpHeight = 5;
        runSpeed = 40;
        turnSpeed = 20;

        characterPos = new(0, -0.6f, 0.35f);
        camPos = new(0, -0.25f, -1.75f);
        camDir = new(35, 0, 0);
    }
}
