using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovementObject
{

    // Start is called before the first frame update

    private const float REGISTER_PRESS_IN = .15f;
    private const float REGISTER_INTERACT_IN = .15f;

    private float fButtonPressTime = 0;

    private UIManager uiManager;
    private GameController gc;

    private void Awake()
    {
        uiManager = GameObject.FindGameObjectWithTag(MMConstants.TAG_GAME_CONTROLLER).GetComponent<UIManager>();
        gc = GameObject.FindGameObjectWithTag(MMConstants.TAG_GAME_CONTROLLER).GetComponent<GameController>();
        v3LookDirection = Vector3.forward;
    }
    new void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (bfinishedMoving)
        {
            if(Input.GetAxis(MMConstants.INPUT_HORIZONTAL) > 0)
            {
                ManageMove(Vector3.right);
            }
            else if(Input.GetAxis(MMConstants.INPUT_HORIZONTAL) < 0)
            {
                ManageMove(Vector3.left);

            }
            else if (Input.GetAxis(MMConstants.INPUT_VERTICAL) > 0)
            {
                ManageMove(Vector3.forward);

            }
            else if (Input.GetAxis(MMConstants.INPUT_VERTICAL) < 0)
            {
                ManageMove(Vector3.back);
            }

            GameObject soLookingAt = GetLookingAtObject(v3LookDirection);
            if(soLookingAt.GetComponent<ScenarioObject>().IsInteractable())
            {
                if ((Input.GetAxis(MMConstants.INPUT_TRIGGERS) != 0))
                {

                    ManageInteract(soLookingAt.GetComponent<ScenarioObject>());

                }
                uiManager.EnableInteractionText(soLookingAt.GetComponent<ScenarioObject>().sInteractionText);
            }
            else
            {
                uiManager.DisableInteractionText();
            }

        }

    }

    private void ManageInteract(ScenarioObject so)
    {
        fButtonPressTime += Time.deltaTime;
        if (fButtonPressTime >= REGISTER_INTERACT_IN)
        {
            SetButtonCooldown();
            gc.SaveUndoDatas();
            so.Interact((Input.GetAxis(MMConstants.INPUT_TRIGGERS) > 0), (Input.GetAxis(MMConstants.INPUT_TRIGGERS) < 0));
        }
    }

    private void ManageMove(Vector3 dir)
    {
        fButtonPressTime += Time.deltaTime;
        if (fButtonPressTime >= REGISTER_PRESS_IN)
        {
            SetButtonCooldown();
            gc.SaveUndoDatas();
            Move(dir);
        }
    }

    private void SetButtonCooldown()
    {
        fButtonPressTime = 0;
    }
}
