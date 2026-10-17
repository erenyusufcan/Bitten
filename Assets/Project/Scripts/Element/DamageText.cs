using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshPro textMesh;


    public void SetDamageText(int damage)
    {
        textMesh.text = damage.ToString();
        transform.DOMoveY(transform.position.y + 1, .3f);
        transform.DOScale(0, .2f).SetDelay(.3f);
    }   
}
