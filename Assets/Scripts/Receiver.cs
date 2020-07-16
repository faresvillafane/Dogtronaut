using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MovementObject
{
    public Color32 clrSolution;
    private Color32 clrStart, clrStartLight, clrStartFlare;
    private bool bIsSolved = false;
    public Renderer recPetalRenderer;
    public Renderer recLamp;

    private const int CENTER_RENDERER_MATERIAL = 0;
    private const int PETAL_RENDERER_MATERIAL = 1;
    private const int LAMP_RENDERER_MATERIAL = 0;
    private const int LAMP_LIGHT_RENDERER_MATERIAL = 1;

    private const float UNSOLVE_EVERY_SEC = 1f;

    private float fTimeSinceLastSolve = 0;

    private const float TIME_TO_SOLVE = 2;
    private float fTimeSolving = 0;

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();

        clrStart = recPetalRenderer.materials[PETAL_RENDERER_MATERIAL].GetColor("_Color");
        clrStartLight = recLamp.materials[LAMP_LIGHT_RENDERER_MATERIAL].GetColor("_Color");
        clrStartFlare = recLamp.GetComponentInChildren<Light>().color;
        recLamp.GetComponentInChildren<Light>().enabled = false;
        UpdateColorSolution(clrSolution);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        //if (bIsSolved)
        {
            if (fTimeSinceLastSolve >= UNSOLVE_EVERY_SEC)
            {
                Unsolve();
            }
            fTimeSinceLastSolve += Time.deltaTime;
        }
    }

    public void UpdateColorSolution(Color32 clr)
    {
        clrSolution = clr;
        recLamp.materials[LAMP_RENDERER_MATERIAL].SetColor("_Color", clrSolution);
    }

    public bool IsSolved()
    {
        return bIsSolved;
    }

    public void Solve()
    {
        fTimeSinceLastSolve = 0;
        fTimeSolving += MMConstants.TIME_TO_TRYTOSOLVE_AGAIN;

        if (fTimeSolving >= TIME_TO_SOLVE)
        {
            recLamp.GetComponentInChildren<Light>().enabled = true;
            recPetalRenderer.materials[PETAL_RENDERER_MATERIAL].SetColor("_Color", clrSolution);
            bIsSolved = true;
        }
    }

    public void Unsolve()
    {
        fTimeSinceLastSolve = 0;
        fTimeSolving = 0;
        recPetalRenderer.materials[PETAL_RENDERER_MATERIAL].SetColor("_Color", clrStart);
        recLamp.materials[LAMP_LIGHT_RENDERER_MATERIAL].SetColor("_Color", clrStartLight);
        recLamp.GetComponentInChildren<Light>().enabled = false;

        recLamp.GetComponentInChildren<Light>().color = clrStartFlare;

        bIsSolved = false;
    }

    public void TryToSolve(Color32 clr)
    {
        recLamp.materials[LAMP_LIGHT_RENDERER_MATERIAL].SetColor("_Color", clr);
        recLamp.GetComponentInChildren<Light>().color = clr;
        if (clr.Equals(clrSolution))
        {
            Solve();
        }
    }
}
