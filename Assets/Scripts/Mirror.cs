using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MovementObject
{
    new void Start()
    {
        base.Start();
        
    }
    public override void Interact(bool bBumperRight, bool bBumperLeft)
    {
        base.Interact(bBumperRight, bBumperLeft);
        Rotate22D(bBumperRight);
    }

}
