using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float SmoothTime;
    private Vector3 _vel;
    public float rotationOffset;
    public Camera mainCamera;
    private Vector3 _originalCameraPos;
    private void Start()
    {
        _originalCameraPos = mainCamera.transform.localPosition;
    }

    private void FixedUpdate()
    {
        var targetpos = player.transform.position + player.transform.forward * rotationOffset;
        targetpos.y = 0;
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetpos,
            ref _vel,
            SmoothTime);
    }

    public void ShakeCamera(float duration, float magnitude)
    {
       mainCamera.transform.DOKill();
       mainCamera.transform.localPosition = _originalCameraPos;
       mainCamera.transform.DOShakePosition(duration, magnitude);

    }
}
