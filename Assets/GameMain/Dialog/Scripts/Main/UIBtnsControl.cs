using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBtnsControl : MonoBehaviour
{
    public Button skipBtn;
    public Button autoBtn;
    public Image skipBtnImage;
    public bool isSkip;
    public bool isAuto;
    public Sprite skipNormal;
    public Sprite autoNormal;
    public Sprite skipUsing;
    public Sprite autoUsing;
    private void Awake()
    {
        skipBtnImage = skipBtn.GetComponentInChildren<Image>();
    }
    public void ToggleSkipBtn()
    {
        if(isSkip)
        {
            isSkip = false;
            skipBtnImage.sprite = skipNormal;
        }
        else
        {
            isSkip = true;
            skipBtnImage.sprite = skipUsing;
        }
    }
    public void ToggleAutoBtn()
    {
        if (isAuto)
        {
            isAuto = false;
            autoBtn.GetComponent<Image>().sprite = autoNormal;
        }
        else
        {
            isAuto = true;
            autoBtn.GetComponent<Image>().sprite = autoUsing;
        }
    }
}
