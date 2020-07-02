using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI tmpMainText;
    public TextMeshProUGUI tmpInteractionText;
    public TextMeshProUGUI tmpMenuText;
    private string sInteractionId = "interactionNo";
    private int iNumberOfInteracctionMade = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShownMainText(string sText)
    {

        tmpMainText.text = sText;

        DOTween.Sequence()
                .Append(tmpMainText.DOFade(1, .7f))
                .SetDelay(.5f)
                .Append(tmpMainText.DOFade(0, .5f));
                
        
    }
    public void EnableInteractionText(string sText)
    {
        SetActiveInteraction(sText, true);
    }

    public void DisableInteractionText()
    {
        SetActiveInteraction("", false);
    }

    public void EnableMenu(string sText)
    {
        SetActive(tmpMenuText, sText, true);
    }

    public void DisableMenu()
    {
        SetActive(tmpMenuText, "", false);
    }

    public void SetActiveMenu(string sText, bool bActive)
    {
        SetActive(tmpMenuText, sText, bActive);
    }

    public void SetActiveInteraction(string sText, bool bActive)
    {
        SetActive(tmpInteractionText, sText, bActive);
    }

    public void SetActive(TextMeshProUGUI txt, string sText, bool bActive)
    {
        txt.text = sText;

        txt.DOFade((bActive) ? 1 : 0, .15f).SetId(sInteractionId + iNumberOfInteracctionMade.ToString());
        iNumberOfInteracctionMade++;

    }



}
