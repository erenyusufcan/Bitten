using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public Image bgImage;
    public Image grenadeImage;
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

    public void SetFillBar(float fillAmount)
    {
        grenadeImage.fillAmount = fillAmount;
        Color bgImageColor = Color.Lerp(Color.red, Color.green, fillAmount);
        bgImageColor.a = 0.5f;
        bgImage.color = bgImageColor;   
    }

}
