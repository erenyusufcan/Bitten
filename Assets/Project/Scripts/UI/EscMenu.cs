using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenu : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, .2f).SetUpdate(true);
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }
}
