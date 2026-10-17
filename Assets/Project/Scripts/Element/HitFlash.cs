using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    public List<SkinnedMeshRenderer> skinnedMeshRenderers;
    public Material flashMaterial;
    private List<Material> _originalMaterials = new List<Material>();
    private Coroutine _hitFlashCoroutine;

    private void Start()
    {
        foreach(var skin in skinnedMeshRenderers)
        {
            _originalMaterials.Add(skin.material);
        }
    }
    public void PlayHitFlash()
    {
        if(_hitFlashCoroutine != null)
        {
            StopCoroutine(_hitFlashCoroutine);
        }
        _hitFlashCoroutine = StartCoroutine(HitFlashCoroutine());    
    }

    IEnumerator HitFlashCoroutine()
    {
        ChangeSkinColor();
        yield return new WaitForSeconds(.1f);
        GetBackSkinColor();
    }

   
    private void ChangeSkinColor()
    {
      foreach (var skin in skinnedMeshRenderers)
        {
           skin.material = flashMaterial;
        }
    }
    private void GetBackSkinColor()
    {
        for (int i = 0; i < skinnedMeshRenderers.Count; i++)
        {
            skinnedMeshRenderers[i].material = _originalMaterials[i];
        }
    }

}
