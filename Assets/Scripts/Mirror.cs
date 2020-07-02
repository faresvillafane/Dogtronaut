using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MovementObject
{
    public GameObject[] goStands;
    
    new void Start()
    {
        base.Start();
        print("MIRROR START");
        if (bEnableMovement)
        {
            for (int i = 0; i<goStands.Length;i++)
            {
                goStands[i].SetActive(false);
            }
        }
    }
    public override void Interact(bool bBumperRight, bool bBumperLeft)
    {
        base.Interact(bBumperRight, bBumperLeft);
        Rotate22D(bBumperRight);
    }
}
