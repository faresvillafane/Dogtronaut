using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoData
{
    public Vector3 v3Position;
    public Quaternion qRotation;

    public UndoData(Vector3 v3StartPosition, Quaternion qStartRotation)
    {
        v3Position = v3StartPosition;
        qRotation = qStartRotation;
    }
}
