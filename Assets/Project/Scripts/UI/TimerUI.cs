using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public AudioManager audioManager;
    private CanvasGroup _canvasGroup;
    public TimerManager timerManager;
    public Image timerFillImage;
    public TextMeshProUGUI timeTxt;
    public TextMeshProUGUI warningTimeTxt;
    private float _previousTime;    

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, .2f);
    }
    public void Hide(float time)
    {
        _canvasGroup.DOFade(0, time).OnComplete(() => gameObject.SetActive(false));
    }

    public void TimeFillAmount(float currentTime, float startTime)
    {
        timerFillImage.DOFillAmount(currentTime / startTime, .1f);
        if (currentTime <= 5)
        {
            warningTimeTxt.text = Mathf.Round(currentTime).ToString();
            warningTimeTxt.gameObject.SetActive(true);
            timeTxt.gameObject.SetActive(false);    
        }
        else
        {
            timeTxt.text = Mathf.Round(currentTime).ToString();
            timeTxt.alpha = 1;
            timeTxt.gameObject.SetActive(true);
            warningTimeTxt.gameObject.SetActive(false);
        }
        if (Mathf.Round(currentTime) != _previousTime)
        {
            if(currentTime <= 5)
            {
                warningTimeTxt.transform.localScale = Vector3.zero;
                warningTimeTxt.transform.DOScale(Vector3.one, .2f).SetEase(Ease.OutBack);
                audioManager.PlayWarningTimeSoundAS();
                timerManager.gameDirector.uiManager.getHitUI.ShowGetHitEffect();    
            }
            _previousTime = Mathf.Round(currentTime);
        }



    }
}
