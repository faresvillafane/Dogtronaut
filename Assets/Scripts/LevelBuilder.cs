﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    
    public GameObject goTile;
    public GameObject goRocks;
    public GameObject goMirror;
    public GameObject goSplitter;
    public GameObject goReceiver;
    public GameObject goLaser;
    public GameObject goCharacter;
    public GameObject goMerger;
    public GameObject goBigRock;

    public LevelData[] levels;

    public int iCurrentLevel = 0;

    public GameObject[] scenarioObjects;
    private GameController gameController;
    private UIManager uiManager;

    public CameraManager cameraManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GetComponent<UIManager>();

        gameController = GetComponent<GameController>();
        

    }

    public void Init()
    {
        LevelData curLevel = GetCurrentLevel();
        GenerateBaseLevel(curLevel.levelBuild, curLevel.MaxSize);
        uiManager.ShownMainText(curLevel.levelName);
        gameController.SetLevelStatus(curLevel.levelBuild, curLevel.MaxSize);
    }

    public void DeleteLevel()
    {
        MMUtils.DeleteAllChildren(this.transform);
    }

    public void GenerateBaseLevel(string sLevel, int maxSide)
    {
        cameraManager.PlaceCamera(maxSide);
        int iObjectPlacementNumber = 0;
        int iObjectMovementNumber = 0;
        int iObjectColorNumber = 0;
        string[] sLevelObjects = sLevel.Split(MMConstants.LEVEL_SEPARATOR);
        scenarioObjects = new GameObject[sLevelObjects.Length];
        for(int x = 0; x < maxSide; x++)
        {
            for (int z = 0; z < maxSide; z++)
            {
                GameObject goNewTile = Instantiate(goTile, new Vector3(x, 0.4f, z), Quaternion.identity);
                goNewTile.transform.SetParent(this.transform);
                int index = MMUtils.ArrayIndexesToListIndex(x, z, maxSide);
                if ((MMEnums.TileType)int.Parse(sLevelObjects[index]) == MMEnums.TileType.NONE)
                {
                    goNewTile.SetActive(false);
                }

                GameObject goToInstantiate = null;
                switch ((MMEnums.TileType) int.Parse(sLevelObjects[index]))
                {
                    case MMEnums.TileType.MIRROR:
                        goToInstantiate = goMirror;
                        break;
                    case MMEnums.TileType.LASER:
                        goToInstantiate = goLaser;
                        break;
                    case MMEnums.TileType.RECEIVER:
                        goToInstantiate = goReceiver;

                        break;
                    case MMEnums.TileType.SPLITTER:
                        goToInstantiate = goSplitter;
                        break;
                    case MMEnums.TileType.ROCKS:
                        goToInstantiate = goRocks;
                        break;
                    case MMEnums.TileType.CHARACTER:
                        goToInstantiate = goCharacter;
                        break;
                    case MMEnums.TileType.MERGER:
                        goToInstantiate = goMerger;
                        break;
                    case MMEnums.TileType.BIG_ROCK:
                        goToInstantiate = goBigRock;
                        break;
                }

                if(goToInstantiate != null)
                {
                    GameObject goNewObj = Instantiate(goToInstantiate, new Vector3(x, 1, z) + goToInstantiate.GetComponent<ScenarioObject>().v3Offset, Quaternion.identity);
                    goNewObj.transform.SetParent(this.transform);
                    goNewObj.GetComponent<ScenarioObject>().SetLevelReference(this);
                    scenarioObjects[index] = goNewObj;

                    if (goToInstantiate != goRocks && goToInstantiate != goBigRock)
                    {
                        goNewObj.transform.rotation = Quaternion.Euler(GetCurrentLevel().objRotation[iObjectPlacementNumber]);
                        goNewObj.name += iObjectPlacementNumber;
                        iObjectPlacementNumber++;
                        if(goToInstantiate == goLaser)
                        {
                            goNewObj.GetComponent<RayLaser>().UpdateRayColor(GetCurrentLevel().objColor[iObjectColorNumber]);
                            iObjectColorNumber++;

                        }
                        else if (goToInstantiate == goReceiver)
                        {
                            goNewObj.GetComponent<Receiver>().UpdateColorSolution(GetCurrentLevel().objColor[iObjectColorNumber]);
                            iObjectColorNumber++;
                        }
                    }

                    if (MMUtils.IsPushableObject(goToInstantiate.GetComponent<ScenarioObject>().tTileType))
                    {
                        goNewObj.GetComponent<MovementObject>().bEnableMovement = GetCurrentLevel().objMovementEnabled[iObjectMovementNumber];
                        iObjectMovementNumber++;
                    }

                }
                else if(!goNewTile.activeInHierarchy)
                {
                    scenarioObjects[index] = goNewTile;
                }
                else
                {
                    scenarioObjects[index] = goNewTile;
                }

            }

        }
    }

    public LevelData GetCurrentLevel()
    {
        return levels[iCurrentLevel];
    }

    public void NextLevel()
    {
        iCurrentLevel = (iCurrentLevel == levels.Length - 1) ? iCurrentLevel : iCurrentLevel + 1;
    }

}
