using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioObject : MonoBehaviour
{
    public MMEnums.TileType tTileType;

    public bool bIsInteractable = false;

    [TextArea]
    public string sInteractionText = "";

    public Vector3 v3Offset = Vector3.zero;
    protected LevelBuilder levelBuilder;

    public bool bEnableRandomYRotation = false;
    public bool bFree = true;
    public int iStepRotation = 360;

    public void Start()
    {
        if (bEnableRandomYRotation)
        {
            int iSteps = 360 / iStepRotation;
            float fRandRot = (Random.value * iSteps);
            if (!bFree)
            {
                fRandRot = (int)fRandRot;
            }
            transform.rotation = Quaternion.Euler(0, fRandRot * iStepRotation, 0);
        }
    }
    public void SetLevelReference(LevelBuilder lb)
    {
        levelBuilder = lb;
    }

    public bool IsInteractable()
    {
        return bIsInteractable;
    }

    public virtual void Interact(bool bBumperRight, bool bBumperLeft)
    {
    }

}
