using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void Start()
    {
        transform.DOMoveY(transform.position.y + .5f, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
}
