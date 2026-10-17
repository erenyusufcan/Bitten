using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
   public static GameDirector instance;
   public LevelManager levelManager;
   public WeaponManager weaponManager;  
   public FXManager fxManager;  
   public AudioManager audioManager;
   public UIManager uiManager;  
   public Player player;
   public TimerManager timerManager;
   public GameState gameState;
   public Texture2D cursorTexture;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            LoadPreviousLevel();
        }
        if(Input.GetKeyDown(KeyCode.Escape) && gameState != GameState.Video)
        {
           uiManager.ShowEscMenu();
           gameState = GameState.EscMenu;
           audioManager.StopGameMusic();

        }
    }
    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        instance = this;    
        gameState = GameState.MainMenu; 
        uiManager.ShowMainMenu();
        uiManager.HideWinMenu();
        uiManager.HideLoseMenu();
        uiManager.HideTimer(.2f);
        uiManager.HideInventoryUI();
        uiManager.HideGrenadeUI();
        uiManager.HideBulletUI();
        uiManager.HideLevelNoUI();
    }

    public void LoadPreviousLevel()
    {
        levelManager.currentlevel--;
        levelManager.currentlevel=Mathf.Clamp(levelManager.currentlevel, 1, levelManager.levels.Count);   
        RestartLevel();
    }

    public void LoadNextLevel()
    {
        levelManager.currentlevel++;
        RestartLevel();
    }

    public void RestartLevel()
    {
        gameState = GameState.Playing;
        levelManager.RestartLevelManager();
        weaponManager.RestartWeaponManager();
        player.RestartPlayer();
        uiManager.ShowTimer();
        uiManager.ShowBulletUI();
        uiManager.ShowLevelNoUI();
        uiManager.ShowInventoryUI();
        uiManager.ShowGrenadeUI();  
        timerManager.StartLevelTimer(levelManager.GetLevelTime());
        audioManager.PlayGameMusic();
        audioManager.StopLevelLoseAS();
        audioManager.StopLevelWinAS();
    }


    public void LevelWin()
    {
        gameState = GameState.Win;
        uiManager.ShowWinMenu();
        uiManager.HideGetHitUI();
        uiManager.HideGrenadeUI();
        uiManager.HideInventoryUI();
        uiManager.HideBulletUI();
        uiManager.HideLevelNoUI();
        uiManager.HideMessageUI();
        weaponManager.CanShootFalseFunc();
        levelManager.StopLevel();
        uiManager.HideTimer(3);
        audioManager.StopGameMusic();
        audioManager.PlayLevelWinAS();  
        

    }
    public void LevelLose(float time)
    {
        gameState = GameState.Lose;
        weaponManager.CanShootFalseFunc();
        uiManager.ShowLoseMenu(time);
        uiManager.HideTimer(time);
        uiManager.HideGetHitUI();
        uiManager.HideGrenadeUI();
        uiManager.HideInventoryUI();
        uiManager.HideBulletUI();
        uiManager.HideLevelNoUI();
        uiManager.HideMessageUI();
        levelManager.StopLevel();
        audioManager.StopGameMusic();
        audioManager.PlayLevelLoseAS(); 
    }
}

public enum GameState
{
    MainMenu,  
    Video,
    EscMenu,
    Playing,
    Win,
    Lose,
    Inventory
}   
