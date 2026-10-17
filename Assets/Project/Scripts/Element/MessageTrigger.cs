using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    public string message;
    public float duration;
    public float delay;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if(player.gameDirector.levelManager.currentlevel < player.gameDirector.levelManager.levels.Count)
            {
                player.gameDirector.uiManager.messageUI.ShowMessage(message, duration, delay);
                gameObject.SetActive(false);
            }
           
        }
    }
}
