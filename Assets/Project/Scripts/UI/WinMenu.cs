using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>(); 
    }

    public void Show()
    {
        Invoke("ActivePanel", 4f);
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }
    public void ActivePanel()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, 1f);
    }
}
