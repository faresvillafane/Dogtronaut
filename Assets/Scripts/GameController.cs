using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject[] receivers;

    public bool bIsComplete = false;

    private const float CHECK_WIN_CONDITION = 1;
    private float fCurrentTime = 0;
    private int[,] iLevelStatus;

    private bool bToNextLevel = false;

    private LevelBuilder levelBuilder;
    private UIManager uiManager;

    public GameObject canvas;

    public MovementObject[] mo;

    public int iCurrentLevel = 0;

    public GameObject[] scenarioObjects;

    public bool bInLevel = false;

    private MenuController mc;

    private const float REGISTER_PRESS_IN = .25f;
    private float fButtonPressTime = 0;

    public List<UndoMatrix> undoScenarioObjects = new List<UndoMatrix>();

    void Start()
    {
        canvas.SetActive(true);
        levelBuilder = GetComponent<LevelBuilder>();
        uiManager = GetComponent<UIManager>();
        mc = GetComponent<MenuController>();
    }

    public void Init()
    {
        levelBuilder.Init();
        bInLevel = true;
        uiManager.EnableMenu(MMConstants.LANG_MENU);
        bToNextLevel = false;
        bIsComplete = false;
        receivers = GameObject.FindGameObjectsWithTag(MMConstants.TAG_RECEIVER);
        mo = GetComponentsInChildren<MovementObject>();
        SaveUndoDatas();
    }

    public void SetLevelStatus(string sLevel, int iLevelDim)
    {
        string[] sLevelArray = sLevel.Split(MMConstants.LEVEL_SEPARATOR);
        iLevelStatus = new int[iLevelDim, iLevelDim];
        for (int x = 0; x < iLevelDim; x++)
        {
            for (int z = 0; z < iLevelDim; z++)
            {
                int index = MMUtils.MatrixIndexesToListIndex(x,z,iLevelDim);
                iLevelStatus[x, z] = int.Parse(sLevelArray[index]);
            }
        }
    }

    void Update()
    {
        if (bInLevel)
        {
            fCurrentTime += Time.deltaTime;
            if (!bIsComplete && fCurrentTime >= CHECK_WIN_CONDITION)
            {
                fCurrentTime = 0;
                bool bIsCompletePrev = CheckLevelCompletion();
                bIsComplete = bIsCompletePrev;
            }
            else if (!bToNextLevel && bIsComplete)
            {

                bToNextLevel = true;
                uiManager.ShownMainText(MMConstants.LANG_LEVEL_COMPLETE);
                uiManager.DisableMenu();

                StartCoroutine(PrepareNextLevel());
            }

            if (Input.GetButton(MMConstants.INPUT_BUMPER_LEFT))
            {
                fButtonPressTime += Time.deltaTime;
                if(fButtonPressTime >= REGISTER_PRESS_IN)
                {
                    fButtonPressTime = 0;
                    ResetLevel();
                }
            }

            if (Input.GetKey(KeyCode.U))
            {
                fButtonPressTime += Time.deltaTime;
                if (fButtonPressTime >= REGISTER_PRESS_IN)
                {
                    fButtonPressTime = 0;
                    StartCoroutine(Undo());
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (bInLevel)
            {
                bInLevel = false;
                uiManager.DisableAllMenus();
                mc.Init();
            }
            else
            {
                Application.Quit();
            }
        }
    }

    private bool CheckLevelCompletion()
    {
        bool bIsComplete = true;
        for(int i = 0; receivers != null && i < receivers.Length; i++)
        {
            bIsComplete &= receivers[i].GetComponentInParent<Receiver>().IsSolved();
        }

        return bIsComplete;
    }

    public IEnumerator PrepareNextLevel()
    {
        yield return new WaitForSeconds(2f);
        levelBuilder.DeleteLevel();
        NextLevel();
        Init();

    }

    public void ResetLevel()
    {
        levelBuilder.DeleteLevel();
        Init();
    }

    public void StartLevel(int iLvl)
    {
        levelBuilder.DeleteLevel();
        iCurrentLevel = iLvl;
        Init();
    }

    public bool FinishedMovingAllObjects()
    {
        bool bRes = true;
        for(int i = 0; i < mo.Length && bRes; i++)
        {
            bRes = mo[i].FinishedMoving();
        }
        return bRes;
    }

    public LevelData GetCurrentLevel()
    {
        return levelBuilder.levels[iCurrentLevel];
    }

    public void NextLevel()
    {
        iCurrentLevel = (iCurrentLevel == levelBuilder.levels.Length - 1) ? iCurrentLevel : iCurrentLevel + 1;
    }
    public void SaveUndoDatas()
    {
        for (int i = 0; i < mo.Length ; i++)
        {

            mo[i].SaveUndoData(); 
        }

        UndoMatrix um = new UndoMatrix();
        um.scenarioObjects = new GameObject[scenarioObjects.Length];
        um.scenarioObjects = (GameObject[])scenarioObjects.Clone();
        undoScenarioObjects.Add(um);
    }

    

    public IEnumerator Undo()
    {
        Player player = GetComponentInChildren<Player>();
        mo = mo.OrderBy((d) => (d.transform.position - player.transform.position).sqrMagnitude).ToArray();

        for (int i = 0; i < mo.Length ; i++)
        {
            
            mo[i].LoadLastUndoData();
            yield return new WaitForEndOfFrame();
        }

        if (undoScenarioObjects.Count > 1)
        {
            scenarioObjects = undoScenarioObjects[undoScenarioObjects.Count - 1].scenarioObjects;
            undoScenarioObjects.RemoveAt(undoScenarioObjects.Count - 1);
        }
    }
}
