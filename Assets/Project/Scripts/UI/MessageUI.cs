using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
   private CanvasGroup _canvasGroup;
   public TextMeshProUGUI messageText;

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

    public void ShowMessage(string message,float duration,float delay)
    {
       StartCoroutine(MessageCorotuine(message, duration, delay));  
    }
    IEnumerator MessageCorotuine(string message, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        messageText.DOKill();
        messageText.text = message;
        messageText.DOFade(1, .5f);
        messageText.DOFade(0, .5f).SetDelay(duration);
    }
}
