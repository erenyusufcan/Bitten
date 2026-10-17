using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool _isPlayerInRange;
    private bool _isDoorOpen;
    public Transform leftDoor;
    public Transform rightDoor;

    private void Update()
    {
        if(_isPlayerInRange && GetComponentInParent<Level>().IsLevelCompleted())
        {
            OpenDoor();
           
        }  
        
    }
    private void OpenDoor()
    {
        _isDoorOpen = true;
        leftDoor.DOLocalMoveZ(1.7f, 1f);
        rightDoor.DOLocalMoveZ(-3.15f, 1f);  
    }

    private void CloseDoor()
    {
        _isDoorOpen = false;    
        leftDoor.DOLocalMoveZ(0f, 1f);  
        rightDoor.DOLocalMoveZ(-1.43f, 1f); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))          
        {
            _isPlayerInRange = true; 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
        }
    }


}
