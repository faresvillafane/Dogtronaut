using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merger : MovementObject
{

    private List<MergerSolution> msMergerSolutions = new List<MergerSolution>();
    private const int MAX_MERGER = 2;

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

    public int GetSolutionIndex(Vector3 v3newLaserDiection, Color32 clrLaserColor)
    {
        int iSolIndex = -1;
        for (int i = 0; i < msMergerSolutions.Count; i++)
        {

            if (msMergerSolutions[i].v3LaserDirection == v3newLaserDiection && msMergerSolutions[i].clrLaserColor.Equals(clrLaserColor))
            {
                iSolIndex = i;
            }
        }
        return iSolIndex;
    }

    public void RefreshSolution(int index)
    {
        msMergerSolutions[index].fTimeSinceLastSolve = 0;
    }
    public void AddSolution(MergerSolution ssNewMergerSolution, Vector3 v3PrevDirection, GameObject goLaser)
    {
        /*Color32[] clrsToSplit = MMUtils.GetMergedColor(ssNewSpliterSolution.clrLaserColor);

        if (bInvertRays)
        {
            clrsToSplit.Reverse();
        }

        ssNewSpliterSolution.tLasers = new Transform[clrsToSplit.Length];

        ssSpliterSolutions.Add(ssNewSpliterSolution);


        float fAngleInBetween = 90;//ANGLE_TO_SPLIT / clrsToSplit.Length;
        float fStartingAngle = -45;//ANGLE_TO_SPLIT + fAngleInBetween;
        for (int i = 0; i < clrsToSplit.Length; i++)
        {
            GameObject go = Instantiate(goLaser, transform.position + goLaser.GetComponent<ScenarioObject>().v3Offset - this.v3Offset, Quaternion.LookRotation(v3PrevDirection));
            go.transform.Rotate(new Vector3(0, fStartingAngle, 0), Space.Self);
            fStartingAngle += fAngleInBetween;
            go.transform.SetParent(transform);
            ssNewSpliterSolution.tLasers[i] = go.transform;
            go.GetComponent<RayLaser>().UpdateRayColor(clrsToSplit[i]);
            go.GetComponent<RayLaser>().SetCilinderActive(false);
        }*/

    }
    public void TryToMerge(MergerSolution ssNewMergerSolution, Vector3 v3PrevDirection, GameObject goLaser)
    {
        int idx = GetSolutionIndex(ssNewMergerSolution.v3LaserDirection, ssNewMergerSolution.clrLaserColor);
        if (idx == -1)
        {
            if (msMergerSolutions.Count <= MAX_MERGER)
            {
                AddSolution(ssNewMergerSolution, v3PrevDirection, goLaser);
            }
        }
        else
        {
            RefreshSolution(idx);
        }
    }

}
