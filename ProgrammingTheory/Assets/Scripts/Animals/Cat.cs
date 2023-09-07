using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE: it inherits from the base animal
public class Cat : Animal
{
    // POLYMORPHISM: overriding the base start method, to set variables to values specific to this animal
    protected override void Start()
    {
        base.Start();
        jumpHeight = 50;
        runSpeed = 40;
        turnSpeed = 20;

        characterPos = new();
        camPos = new();
        camDir = new();
    }
}
