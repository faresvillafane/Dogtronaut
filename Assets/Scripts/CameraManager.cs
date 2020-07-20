using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private Player pPlayer;

    private float fFollowSpeed = .5f;
    private Vector3 v3StartPosition;
    // Start is called before the first frame update
    void Awake()
    {
        v3StartPosition = this.transform.position;
    }

    public void PlaceCamera(int iMaxSize, GameObject goCenterTile)
    {
        Vector3 v3CenterTile = new Vector3(iMaxSize/2,.4f, iMaxSize / 2);
        this.transform.position = v3CenterTile + new Vector3(0, iMaxSize, -iMaxSize);
        transform.LookAt(goCenterTile.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
