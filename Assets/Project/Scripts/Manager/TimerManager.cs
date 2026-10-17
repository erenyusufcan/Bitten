using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private float _currentTime; 
    private float _startTime;
    public GameDirector gameDirector;
    public Player player;
    public TimerUI timerUI;

    public void StartLevelTimer(float time)
    {
        _currentTime = time + ((gameDirector.levelManager.currentlevel - 1) / gameDirector.levelManager.levels.Count)*2;
        _startTime = time;
    }

    private void Update()
    {
        if(gameDirector.gameState != GameState.Playing)
        {
            return;
        }   
        _currentTime -= Time.deltaTime;
        timerUI.TimeFillAmount(_currentTime ,_startTime);
        if(_currentTime <= 0)
        {
          gameDirector.LevelLose(3);
          player.TimeFailAnim();
        }   
    }

    public void CollectableCollected()
    {
        _currentTime += 3f;
        _currentTime = Mathf.Clamp(_currentTime, 0, _startTime);
    }
}
