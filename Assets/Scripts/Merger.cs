using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merger : MovementObject
{

    private bool bIsMerging = false;
    public Color32 clrMerge1 = MMConstants.RED;
    public Color32 clrMerge2 = MMConstants.RED;

    public Vector3 v3Ray1Direction = Vector3.forward;
    public Vector3 v3Ray2Direction = Vector3.forward;

    private const float UNMERGE_EVERY_SEC = .01f;

    private float fTimeSinceLastMerge = 0;

    private bool bReceivingRay1 = false;
    private bool bReceivingRay2 = false;

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        if (bIsMerging)
        {
            if (fTimeSinceLastMerge >= UNMERGE_EVERY_SEC)
            {
                Unmerge();
            }
            fTimeSinceLastMerge += Time.deltaTime;

        }

    }

    public void Unmerge()
    {

    }


}
