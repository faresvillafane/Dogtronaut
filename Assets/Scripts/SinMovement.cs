using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SinMovement : MonoBehaviour
{
    public enum MoveType { MOVEY, SCALE};

    public MoveType mt = MoveType.MOVEY;
    private Vector3 v3StartingV3;
    public float fOffset = .25f;
    public float fSpeed = 2f;
    public Ease ease = Ease.Linear;
    public MovementObject mo;

    private float fSinValue = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (mt == MoveType.MOVEY)
        {
            v3StartingV3 = transform.position;
        }
        else if (mt == MoveType.SCALE)
        {
            v3StartingV3 = transform.localScale;
        }
    }
 
    public void Update()
    {
        fSinValue = Mathf.Sin(Time.time * fSpeed) * fOffset;
        if (mo.bEnableMovement)
        {
            if (mt == MoveType.MOVEY)
            {
                transform.position = new Vector3(transform.position.x, fSinValue + v3StartingV3.y, transform.position.z);
            }
        }

        if (mt == MoveType.SCALE)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x + fSinValue, transform.localScale.z);

        }
    }


}
