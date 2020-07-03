using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MovementObject
{
    private const float UNSPLIT_EVERY_SEC = .1f;

    private List<SplitterSolution> ssSpliterSolutions = new List<SplitterSolution>();
    private const int MAX_SPLITTER = 10;
    // Update is called once per frame
    private bool bDeletedThisRound = false;
    new void Update()
    {
        base.Update();
        if (IsSplitting())
        {
            CheckAndRemoveForPastSplits();
        }
    }

    private void CheckAndRemoveForPastSplits()
    {
        bDeletedThisRound = false;
        for (int i = 0; i < ssSpliterSolutions.Count && !bDeletedThisRound; i++)//  SplitterSolution ss in ssSpliterSolutions)
        {
            ssSpliterSolutions[i].fTimeSinceLastSolve += Time.deltaTime;
            if (ssSpliterSolutions[i].fTimeSinceLastSolve >= UNSPLIT_EVERY_SEC)
            {
                for (int j = 0; j< ssSpliterSolutions[i].tLasers.Length;j++)
                {
                    Destroy(ssSpliterSolutions[i].tLasers[j].gameObject);
                }


                ssSpliterSolutions.Remove(ssSpliterSolutions[i]);
                bDeletedThisRound = true;
            }
        }
    }

    public override void Interact(bool bBumperRight, bool bBumperLeft)
    {
        base.Interact(bBumperRight, bBumperLeft);
        MirrorSolutions();
    }

    public void MirrorSolutions()
    {
        for (int i = 0; i < ssSpliterSolutions.Count; i++)
        {
            Quaternion qAuxRotation = ssSpliterSolutions[i].tLasers[0].transform.rotation;
            ssSpliterSolutions[i].tLasers[0].transform.rotation = ssSpliterSolutions[i].tLasers[1].transform.rotation;
            ssSpliterSolutions[i].tLasers[1].transform.rotation = qAuxRotation;
        }
    }

    public bool IsSplitting()
    {
        return ssSpliterSolutions.Count > 0;
    }

    public int GetSolutionIndex(Vector3 v3newLaserDiection, Color32 clrLaserColor)
    {
        int iSolIndex = -1;
        for (int i = 0;  i < ssSpliterSolutions.Count; i++)
        {
            
            if (ssSpliterSolutions[i].v3LaserDirection == v3newLaserDiection && ssSpliterSolutions[i].clrLaserColor.Equals(clrLaserColor))
            {
                iSolIndex = i;
            }
        }
        return iSolIndex;
    }

    public void AddSolution(SplitterSolution ssNewSpliterSolution, Vector3 v3PrevDirection, GameObject goLaser)
    {
        Color32[] clrsToSplit = MMUtils.ColorSplitter(ssNewSpliterSolution.clrLaserColor);

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
        }
    }


    public void RefreshSolution(int index)
    {
        ssSpliterSolutions[index].fTimeSinceLastSolve = 0;
    }

    public void TryToSplit( SplitterSolution ssNewSplitterSolution, Vector3 v3PrevDirection, GameObject goLaser)
    {
        int idx = GetSolutionIndex(ssNewSplitterSolution.v3LaserDirection, ssNewSplitterSolution.clrLaserColor);
        if (idx == -1)
        {
            if(ssSpliterSolutions.Count <= MAX_SPLITTER)
            {
                AddSolution(ssNewSplitterSolution, v3PrevDirection, goLaser);
            }
        }
        else
        {
            RefreshSolution(idx);
        }
    }

}
