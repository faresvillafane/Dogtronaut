using System.Collections;
using System.Collections.Generic;
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


    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(true);
        levelBuilder = GetComponent<LevelBuilder>();
        uiManager = GetComponent<UIManager>();
        Init();
    }

    public void Init()
    {
        levelBuilder.Init();
        uiManager.EnableMenu(MMConstants.LANG_MENU);
        bToNextLevel = false;
        bIsComplete = false;
        receivers = GameObject.FindGameObjectsWithTag(MMConstants.TAG_RECEIVER);

    }

    public void SetLevelStatus(string sLevel, int iLevelDim)
    {
        string[] sLevelArray = sLevel.Split(MMConstants.LEVEL_SEPARATOR);
        iLevelStatus = new int[iLevelDim, iLevelDim];
        for (int x = 0; x < iLevelDim; x++)
        {
            for (int z = 0; z < iLevelDim; z++)
            {
                int index = MMUtils.ArrayIndexesToListIndex(x,z,iLevelDim);
                iLevelStatus[x, z] = int.Parse(sLevelArray[index]);
            }
        }
    }

    // Update is called once per frame
    void Update()
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

            StartCoroutine(NextLevel());
        }

        if (Input.GetButton(MMConstants.INPUT_BUMPER_LEFT))
        {
            ResetLevel();
        }
    }

    private bool CheckLevelCompletion()
    {
        bool bIsComplete = true;
        for(int i = 0; i < receivers.Length; i++)
        {
            bIsComplete &= receivers[i].GetComponentInParent<Receiver>().IsSolved();
        }

        return bIsComplete;
    }

    public IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2f);
        levelBuilder.DeleteLevel();
        levelBuilder.NextLevel();
        Init();

    }

    public void ResetLevel()
    {
        levelBuilder.DeleteLevel();
        Init();
    }
}
