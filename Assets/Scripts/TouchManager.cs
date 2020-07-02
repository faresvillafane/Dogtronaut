using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    void Update()
    {
        if (MMConstants.DEBUG_MODE)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if(hit.collider.tag == MMConstants.TAG_MIRROR ||
                        hit.collider.tag == MMConstants.TAG_SPLITTER)
                    {
                        hit.transform.GetComponent<MovementObject>().Rotate22D();
                    }
                }
            }
        }
    }
}