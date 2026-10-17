using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public List<Level> levels;
    private Level _currentLevel;
    public int currentlevel;
    public void RestartLevelManager()
    {
        DeleteCurrentLevel();
        InstantiateNewLevel();
    }

    private void InstantiateNewLevel()
    {
        _currentLevel = Instantiate(levels[(currentlevel - 1) % levels.Count]);
        _currentLevel.transform.position = Vector3.zero;
        _currentLevel.StartLevel(gameDirector.player);
        gameDirector.uiManager.levelNoUI.SetLevelNo(currentlevel);
    }

    private void DeleteCurrentLevel()
    {
        if (_currentLevel)
        {
            Destroy(_currentLevel.gameObject);
        }
    }

    public void StopLevel()
    {
        _currentLevel.StopLevel();
    }

    public float GetLevelTime()
    {
       return _currentLevel.LevelTime;
    }
}
