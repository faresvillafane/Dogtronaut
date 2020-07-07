using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(menuName = "Level/Level Asset")]
public class LevelData : ScriptableObject
{
    public int LevelNumber;
    public string levelName;
    public string description;

    public MMEnums.LevelDifficulty levelDifficulty = MMEnums.LevelDifficulty.TUTORIAL;

    [TextArea]
    public string levelBuild;

    public Vector3[] objRotation;
    public Color[] objColor;
    public bool[] objMovementEnabled;

    public int MaxSize = 5;


}

/*
    "5,5,5,5,5,5,5,5,2,5," +
    "5,0,0,0,0,0,0,0,0,5," +
    "5,0,3,0,0,0,0,0,3,5," +
    "5,0,0,0,0,0,0,0,0,5," +
    "5,0,0,0,0,0,0,0,0,5," +
    "5,0,0,0,0,0,0,0,0,5," +
    "5,0,0,0,0,0,0,0,0,5," +
    "5,0,0,0,0,0,0,0,0,5," +
    "1,0,0,0,0,0,0,0,3,2," +
    "5,5,5,5,5,5,5,5,5,5";

    "-1,-1,-1,-1,-1,-1,-1,-1,-1,-1," +
    "-1,-1,-1,-1,-1,-1,-1,-1,-1,-1," +
    "-1,-1,-1,-1,-1,-1,-1,-1,-1,-1," +
    "0,0,0,-1,-1,-1,-1,0,0,-1," +
    "-1,0,0,0,0,0,0,0,0,0," +
    "1,0,0,0,0,0,0,0,0,0," +
    "-1,0,0,0,0,0,0,0,0,0," +
    "0,0,0,-1,-1,-1,-1,0,0,-1," +
    "-1,-1,-1,-1,-1,-1,-1,-1,-1,-1," +
    "-1,-1,-1,-1,-1,-1,-1,-1,-1,-1," +
    "-1,-1,-1,-1,-1,-1,-1,-1,-1,-1," ;
*/
