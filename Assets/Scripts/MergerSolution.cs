using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergerSolution

{
    public Color32 clrLaserColor;
    public Vector3 v3LaserDirection;
    public float fTimeSinceLastSolve;
    public Transform tLaser;

    public MergerSolution(Color32 clrNewLaserColor, Vector3 v3NewLaserDirection)
    {
        clrLaserColor = clrNewLaserColor;
        v3LaserDirection = v3NewLaserDirection;
        fTimeSinceLastSolve = 0;
    }
}
