﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementObject : ScenarioObject
{
    private Quaternion qTargetRotation = Quaternion.Euler(Vector3.zero);

    private Vector3 v3TargetPosition;
    private Vector3 v3CurrentPosition;

    private float fRotatingSpeed = .05f;
    private float fMovementSpeed = .1f;

    protected bool bFinishedRotating = true, bfinishedMoving = true;

    public bool bEnableMovement = true, bEnableRotation = true;

    public bool bRotateBeforeMove = false;

    protected Vector3 v3LookDirection = Vector3.left;

    public GameObject[] goMovementObjects;


    protected new void Start()
    {
        base.Start();
        qTargetRotation = transform.rotation;
        v3CurrentPosition = v3TargetPosition = transform.position;
        SetEnableMovement();
    }

    protected void Update()
    {
        ManageMoves();
    }
    protected void SetEnableMovement()
    {
        if (bEnableMovement)
        {
            for (int i = 0; i< goMovementObjects.Length;i++)
            {
                goMovementObjects[i].SetActive(false);
            }
        }

    }

    private void ManageMoves()
    {

        if (!bFinishedRotating)
        {
            bFinishedRotating = true;

            bFinishedRotating = RotateObject();
        }
        if (!bfinishedMoving)
        {
            bfinishedMoving = true;
            bfinishedMoving = MoveObject();
        }
    }

    private bool MoveObject()
    {
        if ( !(MMUtils.AtTarget(transform.position.x, v3TargetPosition.x) && MMUtils.AtTarget(transform.position.z, v3TargetPosition.z)))
        {
            transform.position = Vector3.Lerp(transform.position, this.GetTargetMovement(), this.GetMovementSpeed());
            return false;
        }
        else
        {
            transform.position = GetTargetMovement();
            v3CurrentPosition = GetTargetMovement();
            return true;
        }

    }

    private bool RotateObject()
    {
        if (Quaternion.Angle(this.transform.rotation, this.GetTargetRotation()) >= 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, this.GetTargetRotation(), this.GetRotationSpeed());
            return false;
        }
        else
        {
            transform.rotation = GetTargetRotation();
            return true;
        }

    }

    public bool FinishedMoving()
    {
        return bfinishedMoving;
    }

    public void Move(Vector3 v3Direction)
    {
        if (bEnableMovement)
        {
            if (bRotateBeforeMove && v3LookDirection != v3Direction)
            {
                v3LookDirection = v3Direction;
                if (v3LookDirection == Vector3.right)
                {
                    this.transform.rotation = Quaternion.Euler(Vector3.up * 180);
                }
                else if (v3LookDirection == Vector3.left)
                {
                    this.transform.rotation = Quaternion.Euler(Vector3.zero);
                }
                else if (v3LookDirection == Vector3.forward)
                {
                    this.transform.rotation = Quaternion.Euler(Vector3.up * 90);
                }
                else if (v3LookDirection == Vector3.back)
                {
                    this.transform.rotation = Quaternion.Euler(Vector3.up * 270);
                }

            }
            else
            {
                if (ValidateMove(v3Direction))
                {
                    v3TargetPosition += v3Direction;
                    bfinishedMoving = false;
                    RefreshLevelReference(v3Direction);
                }
                
                else if (ValidatePush(v3Direction))
                {
                    int index = MMUtils.MatrixIndexesToListIndex(v3CurrentPosition + v3Direction, levelBuilder.GetCurrentLevel().MaxSize);
                    MovementObject mvObjectToMove = levelBuilder.scenarioObjects[index].GetComponent<MovementObject>();
                    if (mvObjectToMove.bEnableMovement)
                    {
                        StartCoroutine(mvObjectToMove.Movequeue(v3Direction));
                    }
                }
                
            }
        }
    }

    public IEnumerator Movequeue(Vector3 v3Direction)
    {
        yield return new WaitForEndOfFrame();
        Move(v3Direction);
    }

    protected GameObject GetLookingAtObject(Vector3 v3LookDirection)
    {
        int index = MMUtils.MatrixIndexesToListIndex(v3CurrentPosition + v3LookDirection, levelBuilder.GetCurrentLevel().MaxSize);
        return levelBuilder.scenarioObjects[index];

    }

    private bool ValidateMove(Vector3 v3Direction)
    {
        int index = MMUtils.MatrixIndexesToListIndex(v3CurrentPosition + v3Direction, levelBuilder.GetCurrentLevel().MaxSize);
        return MMUtils.IsWalkableObject(levelBuilder.scenarioObjects[index].GetComponent<ScenarioObject>().tTileType) && levelBuilder.scenarioObjects[index].activeInHierarchy;
    }

    private bool ValidatePush(Vector3 v3Direction)
    {
        int index = MMUtils.MatrixIndexesToListIndex(v3CurrentPosition + v3Direction, levelBuilder.GetCurrentLevel().MaxSize);
        return MMUtils.IsPushableObject(levelBuilder.scenarioObjects[index].GetComponent<ScenarioObject>().tTileType) 
            && levelBuilder.scenarioObjects[index].GetComponent<MovementObject>().bEnableMovement;
    }

    public void RefreshLevelReference(Vector3 v3Direction)
    {
        int iCurrIndex = MMUtils.MatrixIndexesToListIndex(v3TargetPosition, levelBuilder.GetCurrentLevel().MaxSize);
        int iPrevIndex = MMUtils.MatrixIndexesToListIndex(v3CurrentPosition, levelBuilder.GetCurrentLevel().MaxSize);
        GameObject goAux = levelBuilder.scenarioObjects[iCurrIndex];
        levelBuilder.scenarioObjects[iCurrIndex] = levelBuilder.scenarioObjects[iPrevIndex];
        levelBuilder.scenarioObjects[iPrevIndex] = goAux;
    }

    public void Rotate(float fAngle, Vector3 v3Direction)
    {
        if (bEnableRotation)
        {
            SetTargetRotation(GetTargetRotation() * Quaternion.AngleAxis(fAngle, v3Direction));
            bFinishedRotating = false;
        }
    }
    public void Rotate22D(bool bClockWise = true)
    {
        Rotate((bClockWise) ? 1 * 22.5f : -1 * 22.5f, Vector3.up);
    }
    public void Rotate45D(bool bClockWise = true)
    {
        Rotate((bClockWise)? 1 * 45f : -1 * 45f, Vector3.up);
    }

    public float GetRotationSpeed()
    {
        return fRotatingSpeed;
    }

    public Quaternion GetTargetRotation()
    {
        return qTargetRotation;
    }

    public void SetTargetRotation(Quaternion qNewTargetRotation)
    {
        qTargetRotation = qNewTargetRotation;
    }

    public float GetMovementSpeed()
    {
        return fMovementSpeed;
    }

    public Vector3 GetTargetMovement()
    {
        return v3TargetPosition;
    }

    public void SetTargetPosition(Vector3 v3NewTargetPosition)
    {
        v3TargetPosition = v3NewTargetPosition;
    }
}
