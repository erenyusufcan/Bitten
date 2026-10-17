using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelNoUI : MonoBehaviour
{
    public UIManager uiManager;
    private CanvasGroup _canvasGroup;
    public TextMeshProUGUI levelNoText;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

    }
 
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1f, 0.5f);
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0f, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }

    public void SetLevelNo(int currentlevel)
    {
        levelNoText.text = "Level " + currentlevel.ToString();
    }
}
