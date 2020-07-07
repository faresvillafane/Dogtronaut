using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public GameObject prefLevelSelect;
    private Vector3 v3StartPosition = new Vector3(-9,0,0);
    private Vector3 v3CurrentPosition = new Vector3(-9,0,0);
    private LevelBuilder lb;
    private int fOffset = 2;
    private const int MAX_COLUMN = 10;
    public GameObject goCamera;
    private Vector3 v3CameraStartPos;
    private Quaternion qCameraStartRot;


    public Color32[] clrDifficulty;

    public Color[] clrToMerge;
    public Color clrSol;
    // Start is called before the first frame update
    void Start()
    {
        clrSol = MMUtils.ColorMergeFormula(clrToMerge);
        lb = GetComponent<LevelBuilder>();
        v3CameraStartPos = goCamera.transform.position;
        qCameraStartRot = goCamera.transform.rotation;
        Init();
    }

    public void Init()
    {
        v3CurrentPosition = v3StartPosition;

         goCamera.transform.position = v3CameraStartPos;
         goCamera.transform.rotation = qCameraStartRot;

        MMUtils.DeleteAllChildren(this.transform);
        for (int i = 0; i < lb.levels.Length; i++)
        {
            GameObject goLevelSelect = Instantiate(prefLevelSelect, this.transform);
            goLevelSelect.name = i.ToString();
            goLevelSelect.transform.position = v3CurrentPosition;

            Renderer recRenderer = goLevelSelect.GetComponentInChildren<Renderer>();

            recRenderer.material.SetColor("_Color", clrDifficulty[(int)lb.levels[i].levelDifficulty]);

            v3CurrentPosition = v3StartPosition + MMUtils.ListToMatrix(i,MAX_COLUMN) * fOffset;
            goLevelSelect.GetComponentInChildren<TextMesh>().text = i.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
