using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MovementObject
{
    public Color32 clrSolution;
    private Color32 clrStart;
    private bool bIsSolved = false;
    public Renderer recPetalRenderer;
    public Renderer recPotRenderer;

    private const int CENTER_RENDERER_MATERIAL = 0;
    private const int PETAL_RENDERER_MATERIAL = 1;
    private const int POT_RENDERER_MATERIAL = 0;

    private const float UNSOLVE_EVERY_SEC = 1f;

    private float fTimeSinceLastSolve = 0;

    private const float TIME_TO_SOLVE = 1;
    private float fTimeSolving = 0;

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();

        clrStart = recPetalRenderer.materials[PETAL_RENDERER_MATERIAL].GetColor("_Color");
        UpdateColorSolution(clrSolution);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (bIsSolved)
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
        recPotRenderer.materials[POT_RENDERER_MATERIAL].SetColor("_Color", clrSolution);
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
            recPetalRenderer.materials[PETAL_RENDERER_MATERIAL].SetColor("_Color", clrSolution);
            bIsSolved = true;
        }
    }

    public void Unsolve()
    {
        fTimeSinceLastSolve = 0;
        fTimeSolving = 0;
        recPetalRenderer.materials[PETAL_RENDERER_MATERIAL].SetColor("_Color", clrStart);
        bIsSolved = false;
    }

    public void TryToSolve(Color32 clr)
    {
        if (clr.Equals(clrSolution))
        {
            Solve();
        }
    }
}
