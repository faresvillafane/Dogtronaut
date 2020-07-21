using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merger : MovementObject
{

    private List<MergerSolution> msMergerSolutions = new List<MergerSolution>();
    private const int MAX_MERGER = 2;

    private const float UNMERGE_EVERY_SEC = .1f;

    public GameObject mergedLaser;

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (msMergerSolutions.Count > 0)
        {
            CheckAndRemoveForPastSplits();
        }
    }
    public override void Interact(bool bBumperRight, bool bBumperLeft)
    {
        base.Interact(bBumperRight, bBumperLeft);
        Rotate90D(bBumperRight);
    }
    private void CheckAndRemoveForPastSplits()
    {
        bool bDeletedThisRound = false;
        for (int i = 0; i < msMergerSolutions.Count && !bDeletedThisRound; i++)//  SplitterSolution ss in ssSpliterSolutions)
        {
            msMergerSolutions[i].fTimeSinceLastSolve += Time.deltaTime;
            if (msMergerSolutions[i].fTimeSinceLastSolve >= UNMERGE_EVERY_SEC)
            {
                print("DELETE SOLUTION");
                msMergerSolutions.Remove(msMergerSolutions[i]);
                RecalculateSolution();
                bDeletedThisRound = true;
            }
        }
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
    public void AddSolution(MergerSolution ssNewMergerSolution)
    {

        msMergerSolutions.Add(ssNewMergerSolution);

        RecalculateSolution();
    }

    public void RecalculateSolution()
    {
        Color32 clrMerge = MMConstants.WHITE;

        mergedLaser.GetComponent<LineRenderer>().enabled = mergedLaser.GetComponent<RayLaser>().enabled = true;

        if (msMergerSolutions.Count == 0)
        {
            mergedLaser.GetComponent<LineRenderer>().enabled = mergedLaser.GetComponent<RayLaser>().enabled = false;
        }
        else if (msMergerSolutions.Count == 1)
        {
            clrMerge = msMergerSolutions[0].clrLaserColor;
        }
        else
        {
            clrMerge = MMUtils.GetMergedColor(msMergerSolutions[0].clrLaserColor, msMergerSolutions[1].clrLaserColor);
        }
        mergedLaser.GetComponent<RayLaser>().UpdateRayColor(clrMerge);
    }

    public void TryToMerge(MergerSolution ssNewMergerSolution)
    {
        int idx = GetSolutionIndex(ssNewMergerSolution.v3LaserDirection, ssNewMergerSolution.clrLaserColor);
        if (idx == -1)
        {
            if (msMergerSolutions.Count <= MAX_MERGER)
            {
                AddSolution(ssNewMergerSolution);
            }
        }
        else
        {
            RefreshSolution(idx);
        }
    }

}
