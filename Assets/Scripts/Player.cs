﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovementObject
{

    // Start is called before the first frame update

    private const float BUTTON_COOLDOWN = .2f;
    private bool bOnCooldown = false;
    private float fCurrentCooldown = 0;
    private UIManager uiManager;

    private void Awake()
    {
        uiManager = GameObject.FindGameObjectWithTag(MMConstants.TAG_GAME_CONTROLLER).GetComponent<UIManager>();
    }
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        fCurrentCooldown -= Time.deltaTime;
        if(fCurrentCooldown <= 0)
        {
            bOnCooldown = false;
        }
        if (bfinishedMoving && !bOnCooldown)
        {
            if(Input.GetAxis(MMConstants.INPUT_HORIZONTAL) > 0)
            {
                Move(Vector3.right);
                SetButtonCooldown();
            }
            else if(Input.GetAxis(MMConstants.INPUT_HORIZONTAL) < 0)
            {
                Move(Vector3.left);
                SetButtonCooldown();
            }
            else if (Input.GetAxis(MMConstants.INPUT_VERTICAL) > 0)
            {
                Move(Vector3.forward);
                SetButtonCooldown();
            }
            else if (Input.GetAxis(MMConstants.INPUT_VERTICAL) < 0)
            {
                Move(Vector3.back);
                SetButtonCooldown();
            }

            GameObject soLookingAt = GetLookingAtObject(v3LookDirection);
            if(soLookingAt != null && soLookingAt.GetComponent<ScenarioObject>().IsInteractable())
            {
                if ((Input.GetAxis(MMConstants.INPUT_TRIGGERS) != 0))
                {
                    soLookingAt.GetComponent<ScenarioObject>().Interact((Input.GetAxis(MMConstants.INPUT_TRIGGERS) > 0), (Input.GetAxis(MMConstants.INPUT_TRIGGERS) < 0));
                    SetButtonCooldown();
                }
                uiManager.EnableInteractionText(soLookingAt.GetComponent<ScenarioObject>().sInteractionText);
            }
            else
            {
                uiManager.DisableInteractionText();

            }

        }



    }

    private void SetButtonCooldown()
    {
        fCurrentCooldown = BUTTON_COOLDOWN;
        bOnCooldown = true;
    }
}
