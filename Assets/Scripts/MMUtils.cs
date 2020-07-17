using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMUtils : MonoBehaviour
{
    public static  void DeleteAllChildren(Transform t)
    {
        DeleteAllChildrenWithException(t, "");
    }

    public static void DeleteAllChildrenWithException(Transform t, string sNameException)
    {
        int iNumOfChilds = t.childCount;
        for (int i = iNumOfChilds - 1; i >= 0; i--)
        {
            if (t.GetChild(i).name != sNameException)
            {
                GameObject.DestroyImmediate(t.GetChild(i).gameObject);
            }
        }
    }

    public static int MatrixIndexesToListIndex(Vector3 v3Pos, int iDim)
    {
        return (int)v3Pos.z + (int)v3Pos.x * iDim;
    }

    public static Vector3 ListToMatrix(int idx, int maxDim)
    {
        idx++;
        Vector3 v2Sol = new Vector3(idx % maxDim, 0, idx / maxDim);
        return v2Sol;
    }

    public static int MatrixIndexesToListIndex(int x, int z, int iDim)
    {
        return MatrixIndexesToListIndex(new Vector3(x,0,z), iDim);
    }

    public static bool IsColorObject(MMEnums.TileType tt)
    {
        return tt == MMEnums.TileType.RECEIVER || tt == MMEnums.TileType.LASER;
    }

    public static bool AtTarget(float f1, float f2, float fThreshold = 0.01f)
    {
        return Mathf.Abs(f1 - f2) <= fThreshold;
    }

    public static bool IsScenarioObject(MMEnums.TileType tt)
    {
        return (tt == MMEnums.TileType.CHARACTER || tt == MMEnums.TileType.LASER || tt == MMEnums.TileType.MIRROR || tt == MMEnums.TileType.RECEIVER || tt == MMEnums.TileType.SPLITTER || tt == MMEnums.TileType.MERGER || tt == MMEnums.TileType.DUPLICATOR);
    }

    public static bool IsPushableObject(MMEnums.TileType tt)
    {
        return (tt == MMEnums.TileType.MIRROR || tt == MMEnums.TileType.RECEIVER || tt == MMEnums.TileType.SPLITTER || tt == MMEnums.TileType.BIG_ROCK || tt == MMEnums.TileType.MERGER || tt == MMEnums.TileType.DUPLICATOR);
    }

    public static bool IsWalkableObject(MMEnums.TileType tt)
    {
        return (tt == MMEnums.TileType.EMPTY);
    }
    
    public static Color32[] ColorSplitter(Color32 clrToSplit)
    {
        Color32[] clrSplitted;
        if (clrToSplit.Equals(MMConstants.RED) ||
            clrToSplit.Equals(MMConstants.YELLOW) ||
            clrToSplit.Equals(MMConstants.BLUE))
        {
            clrSplitted = new Color32[2];
            clrSplitted[0] = clrToSplit;
            clrSplitted[1] = clrToSplit;
        }
        else if (clrToSplit.Equals(MMConstants.PURPLE))
        {
            clrSplitted = GetPurpleComposition();
        }
        else if (clrToSplit.Equals(MMConstants.GREEN))
        {
            clrSplitted = GetGreenComposition();
        }
        else //GetOrangeComposition
        {
            clrSplitted = GetOrangeComposition();
        }
        return clrSplitted;

    }

    //Secondary Colors
    public static Color32[] GetPurpleComposition()
    {
        Color32[] clrResult = new Color32[2];
        clrResult[0] = MMConstants.BLUE;
        clrResult[1] = MMConstants.RED;
        return clrResult;
    }
    public static Color32[] GetGreenComposition()
    {
        Color32[] clrResult = new Color32[2];
        clrResult[0] = MMConstants.YELLOW;
        clrResult[1] = MMConstants.BLUE;
        return clrResult;
    }

    public static Color32[] GetOrangeComposition()
    {
        Color32[] clrResult = new Color32[2];
        clrResult[0] = MMConstants.RED;
        clrResult[1] = MMConstants.YELLOW;
        return clrResult;
    }

    public static  Color32 GetMergedColor(Color32 clrMerge1, Color32 clrMerge2)
    {
        Color32 clrResult = MMConstants.WHITE;

        if (clrMerge1.Equals(clrMerge2))
        {
            clrResult = clrMerge1;
        }
        else if ((clrMerge1.Equals(MMConstants.RED) && clrMerge2.Equals(MMConstants.YELLOW))
            || (clrMerge1.Equals(MMConstants.YELLOW) && clrMerge2.Equals(MMConstants.RED)))
        {
            clrResult = MMConstants.ORANGE;
        }
        else if ((clrMerge1.Equals(MMConstants.BLUE) && clrMerge2.Equals(MMConstants.RED))
           || (clrMerge1.Equals(MMConstants.RED) && clrMerge2.Equals(MMConstants.BLUE)))
        {
            clrResult = MMConstants.PURPLE;
        }

        else if ((clrMerge1.Equals(MMConstants.YELLOW) && clrMerge2.Equals(MMConstants.BLUE))
           || (clrMerge1.Equals(MMConstants.BLUE) && clrMerge2.Equals(MMConstants.YELLOW)))
        {
            clrResult = MMConstants.GREEN;
        }

        return clrResult;

    }

}
