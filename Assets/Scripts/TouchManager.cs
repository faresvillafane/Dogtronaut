using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    Ray ray;
    Ray rayD;
    RaycastHit hitD;
    RaycastHit hit;
    private GameController gc;
    private void Start()
    {
        gc = GetComponent<GameController>();
    }
    void Update()
    {
        if (MMConstants.DEBUG_MODE)
        {
            rayD = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayD, out hitD))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if(hitD.collider.tag == MMConstants.TAG_MIRROR ||
                        hitD.collider.tag == MMConstants.TAG_SPLITTER)
                    {
                        hitD.transform.GetComponent<MovementObject>().Rotate22D();
                    }
                }
            }
        }
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.tag == MMConstants.TAG_LEVEL_SELECT)
                {
                    gc.StartLevel(int.Parse(hit.transform.name));
                }
            }
        }
    }
}