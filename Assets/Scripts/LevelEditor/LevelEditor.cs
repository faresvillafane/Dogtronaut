using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
[ExecuteInEditMode]
public class LevelEditor : MonoBehaviour
{
    private Transform tContainer;
    public GameObject prefEditorTile, prefTile, prefMirror, prefReceiver, prefSplitter, prefRocks, prefLaser, prefCharacter,prefMerger, prefBigRock;
    [Range(7,50)]
    public int iTileSize = 5;

    public GameObject[] tiles;

    private const float REFRESH_EVERY_SECOND = .15f;
    private float currentTime = 0;

    public LevelData lvlExit;

    private int iNumberOfObjects = 0;
    private int iNumberOfMovementObjects = 0;
    private int iNumberOfColors = 0;


    // Start is called before the first frame update
    void Start()
    {
        tContainer = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Build NEW Level")]
    private void BuildLevel()
    {
        DeleteLevel();
        BuildLevelTiles(iTileSize);
        AddWalls();
    }

    private void BuildLevelTiles(int iTiles)
    {
        tiles = new GameObject[iTiles * iTiles];
        print("building..");
        for (int x = 0; x < iTiles; x++)
        {
            for (int z = 0; z < iTiles; z++)
            {
                GameObject goNewEditorTile = Instantiate(prefEditorTile, new Vector3(x, 0.4f, z), Quaternion.identity);
                goNewEditorTile.transform.SetParent(tContainer);

                int index = MMUtils.MatrixIndexesToListIndex(x, z, iTiles);
                tiles[index] = goNewEditorTile;
            }

        }
    }


    [ContextMenu("DELETE Level")]
    private void DeleteLevel()
    {
        iNumberOfObjects = 0;
        iNumberOfColors = 0;
        iNumberOfMovementObjects = 0;
        MMUtils.DeleteAllChildren(tContainer);
    }
 

    private void AddWalls()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].transform.localPosition.x == 1 ||
                tiles[i].transform.localPosition.z == 1 ||
                tiles[i].transform.localPosition.x == iTileSize -2 ||
                tiles[i].transform.localPosition.z == iTileSize -2)
            {
                tiles[i].GetComponent<Tile>().ttTile = MMEnums.TileType.ROCKS;
            }

            if (tiles[i].transform.localPosition.x == 0 ||
                tiles[i].transform.localPosition.z == 0 ||
                tiles[i].transform.localPosition.x == iTileSize - 1 ||
                tiles[i].transform.localPosition.z == iTileSize - 1)
            {
                tiles[i].GetComponent<Tile>().ttTile = MMEnums.TileType.NONE;
            }
        }
    }

    public void RefreshTiles()
    {

        for (int i=0; i < tiles.Length; i++)
        {
            if (tiles[i].GetComponent<Tile>().HasChangedValue())
            {
                MMUtils.DeleteAllChildrenWithException(tiles[i].transform, "Tile");

                tiles[i].GetComponent<Tile>().SetNewTile();
                if (tiles[i].GetComponent<Tile>().ttTile == MMEnums.TileType.NONE)
                {
                    tiles[i].GetComponent<Tile>().goTile.SetActive(false);
                }
                else
                {
                    tiles[i].GetComponent<Tile>().goTile.SetActive(true);
                    GameObject goToInstantiate = null;
                    switch (tiles[i].GetComponent<Tile>().ttTile)
                    {
                        case MMEnums.TileType.MIRROR:
                            goToInstantiate = prefMirror;
                            break;
                        case MMEnums.TileType.LASER:
                            goToInstantiate = prefLaser;
                            break;
                        case MMEnums.TileType.RECEIVER:
                            goToInstantiate = prefReceiver;
                            break;
                        case MMEnums.TileType.SPLITTER:
                            goToInstantiate = prefSplitter;
                            break;
                        case MMEnums.TileType.ROCKS:
                            goToInstantiate = prefRocks;
                            break;
                        case MMEnums.TileType.CHARACTER:
                            goToInstantiate = prefCharacter;
                            break;
                        case MMEnums.TileType.MERGER:
                            goToInstantiate = prefMerger;
                            break;
                        case MMEnums.TileType.BIG_ROCK:
                            goToInstantiate = prefBigRock;
                            break;
                    }
                    if (goToInstantiate != null)
                    {
                        GameObject goNewObj = Instantiate(goToInstantiate, Vector3.zero , Quaternion.identity);
                        //tiles[i].GetComponent<Tile>().bMovementOn = goToInstantiate.GetComponent<ScenarioObject>().

                        goNewObj.transform.SetParent(tiles[i].transform);
                        goNewObj.transform.localPosition = new Vector3(0,0.2f , 0) + goToInstantiate.GetComponent<ScenarioObject>().v3Offset;
                        goNewObj.transform.localRotation = Quaternion.identity;

                        if (tiles[i].GetComponent<Tile>().ttTile != MMEnums.TileType.ROCKS && tiles[i].GetComponent<Tile>().ttTile != MMEnums.TileType.BIG_ROCK)
                        {
                            iNumberOfObjects++;
                        }

                        if (MMUtils.IsPushableObject(tiles[i].GetComponent<Tile>().ttTile))
                        {
                            iNumberOfMovementObjects++;
                        }

                        if (MMUtils.IsColorObject(tiles[i].GetComponent<Tile>().ttTile))
                        {

                            
                            if (tiles[i].GetComponent<Tile>().ttTile == MMEnums.TileType.LASER)
                            {
                                tiles[i].GetComponentInChildren<LineRenderer>().startColor = tiles[i].GetComponent<Tile>().clrObject;
                                tiles[i].GetComponentInChildren<LineRenderer>().endColor = tiles[i].GetComponent<Tile>().clrObject;

                                Renderer recRenderer = tiles[i].GetComponentInChildren<LineRenderer>().GetComponentsInChildren<Renderer>()[1];

                                recRenderer.material.SetColor("_Color", tiles[i].GetComponent<Tile>().clrObject);
                            }
                            else if (tiles[i].GetComponent<Tile>().ttTile == MMEnums.TileType.RECEIVER)
                            {

                                Renderer recRenderer = tiles[i].GetComponentInChildren<Receiver>().GetComponentsInChildren<Renderer>()[2];
                                recRenderer.materials[0].SetColor("_Color", tiles[i].GetComponent<Tile>().clrObject);

                            }
                            iNumberOfColors++;

                        }


                    }
                }


            }
        }
    }

    [ContextMenu("EXPORT Level")]
    public void GetLevelData()
    {
        lvlExit.levelBuild = "";
        Vector3[] v3ObjectsRotations = new Vector3[iNumberOfObjects];
        Color[] clrObjects = new Color[iNumberOfColors];
        bool[] bMovementObjects = new bool[iNumberOfMovementObjects];

        int iObjIndex = 0, iClrIndex = 0, iObjMoveIndex = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            lvlExit.levelBuild += ((int)tiles[i].GetComponent<Tile>().ttTile).ToString() + MMConstants.LEVEL_SEPARATOR;

            if (MMUtils.IsScenarioObject(tiles[i].GetComponent<Tile>().ttTile))
            {
                v3ObjectsRotations[iObjIndex] = tiles[i].transform.rotation.eulerAngles;
                iObjIndex++;
            }

            if (MMUtils.IsColorObject(tiles[i].GetComponent<Tile>().ttTile))
            {
                clrObjects[iClrIndex] = tiles[i].GetComponent<Tile>().clrObject;

                iClrIndex++;
            }

            if (MMUtils.IsPushableObject(tiles[i].GetComponent<Tile>().ttTile))
            {
                bMovementObjects[iObjMoveIndex] = tiles[i].GetComponent<Tile>().bMovementOn;
                iObjMoveIndex++;
            }
        }

        lvlExit.objColor = new Color[iNumberOfColors];
        lvlExit.objColor = (Color[])clrObjects.Clone();

        lvlExit.objRotation = new Vector3[iNumberOfObjects];
        lvlExit.objRotation = (Vector3[])v3ObjectsRotations.Clone();

        lvlExit.objMovementEnabled = new bool[iNumberOfMovementObjects];
        lvlExit.objMovementEnabled = (bool[])bMovementObjects.Clone();

        lvlExit.levelBuild = lvlExit.levelBuild.Substring(0, lvlExit.levelBuild.Length - 1);
        lvlExit.MaxSize = iTileSize;
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorUtility.SetDirty(lvlExit);
#endif

        print("DATA GENERATED!");
    }

    [ContextMenu("BUILD Level From Data")]
    public void GenerateLevelFromLevelData()
    {
        DeleteLevel();
        iTileSize = lvlExit.MaxSize;

        BuildLevelTiles(lvlExit.MaxSize);

        string[] sLevelData = lvlExit.levelBuild.Split(MMConstants.LEVEL_SEPARATOR);
        int iColorObjectIdx = 0, iRotationIdx = 0, iObjMoveIdx = 0;
        for(int i= 0; i < sLevelData.Length; i++)
        {
            
            tiles[i].GetComponent<Tile>().ttTile = (MMEnums.TileType)int.Parse(sLevelData[i]);
            if (MMUtils.IsColorObject(tiles[i].GetComponent<Tile>().ttTile))
            {

                tiles[i].GetComponent<Tile>().clrObject = lvlExit.objColor[iColorObjectIdx];
                iColorObjectIdx++;
            }
            if (MMUtils.IsScenarioObject(tiles[i].GetComponent<Tile>().ttTile))
            {
                tiles[i].transform.rotation = Quaternion.Euler(lvlExit.objRotation[iRotationIdx]);

                iRotationIdx++;
            }

            if (MMUtils.IsPushableObject(tiles[i].GetComponent<Tile>().ttTile))
            {
                tiles[i].GetComponent<Tile>().bMovementOn = lvlExit.objMovementEnabled[iObjMoveIdx];
                iObjMoveIdx++;
            }

        }
        RefreshTiles();

    }

}
