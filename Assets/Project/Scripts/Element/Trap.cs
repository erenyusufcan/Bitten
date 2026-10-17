using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Saw saw;

    private void Start()
    {
       saw.transform.DOLocalMoveX(2.3f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
       saw.transform.DOLocalRotate(new Vector3(0, 0, 90), .2f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        saw.transform.DOKill();
    }
}
