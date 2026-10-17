using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHitUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public Image  bloodEffectIMG;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, .2f);
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void ShowGetHitEffect()
    {
        bloodEffectIMG.DOKill();
        bloodEffectIMG.color = new Color(1,0,0,0);
        bloodEffectIMG.DOFade(0.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
    }

}
