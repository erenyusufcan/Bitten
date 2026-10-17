using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletUI : MonoBehaviour
{
    public UIManager uiManager;
    private CanvasGroup _canvasGroup;
    private Weapon _currentWeapon;
    public TextMeshProUGUI bulletCountText;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>(); 
     
    }
    public void StartBulletUI(Weapon weapon)
    {
        _currentWeapon = weapon;
    }
    private void Update()
    {
        bulletCountText.text = _currentWeapon.currentBulletCount.ToString();
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

  
}
