using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Image staminaBarImg;
  
    public void ChangeBarFillAmount(float staminaPercent)
    {
        ShowStaminaBar();
        staminaBarImg.DOKill();
        staminaBarImg.DOFillAmount(staminaPercent, .2f);
    }
    public void ShowStaminaBar()
    {
        gameObject.SetActive(true);
    }
    public void HideStaminaBar()
    {
        gameObject.SetActive(false);
    }
}
