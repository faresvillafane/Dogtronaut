using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class RayLaser : ScenarioObject
{
    public Color32 clrLaser = MMConstants.RED;
    public float updateFrequency = 0.1f;
    public int laserDistance;
    private const string spawnedBeamTag = MMConstants.TAG_LINE_SPAWN;
    public int maxBounce;
    public int maxSplit;
    private float timer = 0;
    private LineRenderer mLineRenderer;

    private bool bHit = false;

    private float timeSinceLastTryToSolve = 0;

    public GameObject[] goBase;
    public Renderer recRenderer;

    private const float ANGLE_TO_SPLIT = 90;

    // Use this for initialization
    void Awake()
    {
        timer = 0;
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        StartCoroutine(RedrawLaser());

        UpdateRayColor(clrLaser);

        recRenderer.material.SetColor("_Color", clrLaser);
    }

    public void UpdateRayColor(Color32 clr)
    {
        clrLaser = clr;
        mLineRenderer.startColor = clrLaser;
        mLineRenderer.endColor = clrLaser;

        recRenderer.material.SetColor("_Color", clr);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag != spawnedBeamTag)
        {
            if (timer >= updateFrequency)
            {
                timer = 0;
                StartCoroutine(RedrawLaser());
            }
            timer += Time.deltaTime;

        }
        timeSinceLastTryToSolve += Time.deltaTime;
    }

    IEnumerator RedrawLaser()
    {
        //Debug.Log("Running");
        int laserSplit = 1; //How many times it got split
        int laserReflected = 1; //How many times it got reflected
        int vertexCounter = 1; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.forward; //direction of the next laser
        Vector3 lastLaserPosition = transform.position; //origin of the next laser
        //TODO
        //mLineRenderer.positionCount

        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;

        while (loopActive)
        {
            bHit = Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance);
            if (bHit)
            {
                //print("hit.transform.gameObject.tag: " + hit.transform.gameObject.tag);
            }

            //Debug.Log("Physics.Raycast(" + lastLaserPosition + ", " + laserDirection + ", out hit , " + laserDistance + ")");

            if (bHit && ((hit.transform.gameObject.tag == MMConstants.TAG_MIRROR) || IsRayKiller(hit.transform.gameObject.tag)))
            {

                Vector3 prevDirection = laserDirection;
                laserReflected++;
                vertexCounter += 3;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
                mLineRenderer.SetPosition(vertexCounter - 2, hit.point);
                mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                mLineRenderer.SetWidth(.15f, .15f);
                lastLaserPosition = hit.point;
                laserDirection = Vector3.Reflect(laserDirection, hit.normal);

                if (hit.transform.gameObject.tag == MMConstants.TAG_SPLITTER)
                {

                    
                    SplitterSolution ss = new SplitterSolution(clrLaser, laserDirection);
                    hit.transform.GetComponent<Splitter>().TryToSplit(ss, prevDirection, this.gameObject);
                    
                    loopActive = false;
                }
                else if (hit.transform.gameObject.tag == MMConstants.TAG_DUPLICATOR)
                {


                    SplitterSolution ss = new SplitterSolution(clrLaser, laserDirection);
                    hit.transform.GetComponent<Duplicator>().TryToSplit(ss, prevDirection, this.gameObject);

                    loopActive = false;
                }
                else if (hit.transform.gameObject.tag == MMConstants.TAG_MERGER)
                {
                    MergerSolution ms = new MergerSolution(clrLaser, laserDirection);

                    hit.transform.GetComponentInParent<Merger>().TryToMerge(ms);
                    loopActive = false;
                }
                else if (IsRayKiller(hit.transform.gameObject.tag))
                {

                    loopActive = false;

                }

            }
            else
            {
                //Debug.Log("No Bounce");
                laserReflected++;
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
                //Debug.Log("InitialPos " + lastLaserPosition + " Last Pos" + lastPos);
                mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));

                loopActive = false;
            }

            if (laserReflected > maxBounce)
                loopActive = false;

            if (bHit && hit.transform.gameObject.tag == MMConstants.TAG_RECEIVER)
            {
                if (timeSinceLastTryToSolve >= MMConstants.TIME_TO_TRYTOSOLVE_AGAIN)
                {
                    timeSinceLastTryToSolve = 0;
                    hit.transform.GetComponentInParent<Receiver>().TryToSolve(clrLaser);
                }
            }
        }

        yield return new WaitForEndOfFrame();
    }

    private bool IsRayKiller(string tag)
    {
        return ((tag == MMConstants.TAG_SPLITTER) ||(tag == MMConstants.TAG_WALL) || (tag == MMConstants.TAG_RECEIVER) || (tag == MMConstants.TAG_MERGER) || (tag == MMConstants.TAG_DUPLICATOR) || (tag == MMConstants.TAG_PLAYER) || (tag == MMConstants.TAG_TILE) || (tag == MMConstants.TAG_GENERIC_RAY_KILLER));
    }




    public void SetCilinderActive(bool bActive)
    {
        for(int i = 0; i < goBase.Length; i++)
        {
            goBase[i].SetActive(bActive);
        }
    }




}

/*
//Debug.Log("Bounce");
laserReflected++;
vertexCounter += 3;
mLineRenderer.SetVertexCount(vertexCounter);
mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
mLineRenderer.SetPosition(vertexCounter - 2, hit.point);
mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
mLineRenderer.SetWidth(.15f, .15f);
lastLaserPosition = hit.point;
Vector3 prevDirection = laserDirection;
laserDirection = Vector3.Reflect(laserDirection, hit.normal);

if (hit.transform.gameObject.tag == splitTag)
{
    //Debug.Log("Split");
    if (laserSplit >= maxSplit)
    {
        Debug.Log("Max split reached.");
    }
    else
    {
        //Debug.Log("Splitting...");
        laserSplit++;
        GameObject go = Instantiate(gameObject, hit.point, Quaternion.LookRotation(prevDirection));
        go.name = spawnedBeamTag;
        go.tag = spawnedBeamTag;
        Color32[] clrsToSplit = ColorSplitter(clrLaser);
        go.GetComponent<RayLaser>().UpdateRayColor(clrsToSplit[0]);
    }
}
*/
