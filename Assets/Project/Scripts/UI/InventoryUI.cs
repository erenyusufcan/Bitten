using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public UIManager uiManager;
    private CanvasGroup _canvasGroup;
    private bool _isInventoryOpen =false;
    public List<Button> buttons;
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

    public void InventoryBtnClicked()
    {
        if (_isInventoryOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }

    }
    public void MachineGunBtnClicked()
    {
        uiManager.gameDirector.weaponManager.SelectMachineGun();
    }
    public void ShotGunBtnClicked()
    {

        uiManager.gameDirector.weaponManager.SelectShotGun();

        print("ShotGunBtnClicked");
    }
    private void OpenInventory()
    {
        _isInventoryOpen = true;

        for (int i = 0; i < buttons.Count; i++)
        {
            GameObject buttonObject = buttons[i].gameObject;
            CanvasGroup canvasGroup = buttons[i].GetComponent<CanvasGroup>();
            RectTransform rectTransform = buttons[i].GetComponent<RectTransform>();

            canvasGroup.DOKill();
            rectTransform.DOKill();

            buttonObject.SetActive(true);

            canvasGroup.alpha = 0f;

            canvasGroup.DOFade(1f, .5f);

            rectTransform.DOAnchorPos3DY(-185f - (i * 115f),.5f);
        }
    }

    private void CloseInventory()
    {
        _isInventoryOpen = false;

        for (int i = 0; i < buttons.Count; i++)
        {
            GameObject buttonObject = buttons[i].gameObject;
            CanvasGroup canvasGroup = buttons[i].GetComponent<CanvasGroup>();
            RectTransform rectTransform = buttons[i].GetComponent<RectTransform>();

            canvasGroup.DOKill();
            rectTransform.DOKill();

            canvasGroup.DOFade(0f,.5f).OnComplete(() =>{buttonObject.SetActive(false);});

            rectTransform.DOAnchorPos3DY(-62.5f,.5f);
        }
    }


}
