using UnityEngine;

public class SplitterSolution
{
    public Color32 clrLaserColor;
    public Vector3 v3LaserDirection;
    public float fTimeSinceLastSolve;
    public Transform[] tLasers;
    //TODO ADD PARENT AND IF PARENT IS NO MORE KILL DA CHILDS
    public SplitterSolution(Color32 clrNewLaserColor, Vector3 v3NewLaserDirection)
    {
        clrLaserColor = clrNewLaserColor;
        v3LaserDirection = v3NewLaserDirection;
        fTimeSinceLastSolve = 0;
    }
}
