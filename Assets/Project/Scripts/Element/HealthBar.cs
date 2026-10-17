using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarImg; 
  
    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
    public void ReduceBar(float fillAmount)
    {
        gameObject.SetActive(true);
        healthBarImg.DOKill();
        healthBarImg.DOFillAmount(fillAmount, .2f);
        healthBarImg.color = Color.red;
        healthBarImg.DOColor(Color.white, .1f).SetLoops(2, LoopType.Yoyo);
    }

    public void HideHealthBar()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        healthBarImg.DOKill();
    }
}

