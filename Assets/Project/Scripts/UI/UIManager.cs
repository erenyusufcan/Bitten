using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public MainMenu mainMenu;   
    public WinMenu winMenu; 
    public LoseMenu loseMenu;   
    public EscMenu escMenu;
    public TimerUI timerUI; 
    public GetHitUI getHitUI;
    public InventoryUI inventoryUI;
    public GrenadeUI grenadeUI;
    public MessageUI messageUI;
    public BulletUI bulletUI;   
    public LevelNoUI levelNoUI; 
    public VideoPlayer videoPlayer;
    public Image darkBG;
    public void ShowLevelNoUI()
    {
        levelNoUI.Show();
    }
    public void HideLevelNoUI()
    {
        levelNoUI.Hide();
    }

    public void ShowBulletUI()
    {
        bulletUI.Show();
    }
    public void HideBulletUI()
    {
        bulletUI.Hide();
    }
    public void ShowMainMenu()
    {
        mainMenu.Show();
    }
    public void ShowWinMenu()
    
    {
        winMenu.Show();
    }   
    public void ShowMessageUI()
    {
        messageUI.Show();
    }
    public void HideMessageUI()
    {
        messageUI.Hide();
    }
    public void HideWinMenu()
    {
        winMenu.Hide();
    }   
    public void ShowLoseMenu(float time)
    {
        loseMenu.Show(time);
    }
    public void HideLoseMenu()
    {
        loseMenu.Hide();
    }
    public void ShowTimer()
    {
        timerUI.Show();
    }
    public void HideTimer(float time)
    {
        timerUI.Hide(time);
    }
    public void ShowEscMenu()
    {
        Time.timeScale = 0f;
        escMenu.Show();
    }
    public void HideEscMenu()
    {
        Time.timeScale = 1f;
        escMenu.Hide();
    }   
    public void ShowGetHitUI()
    {
        getHitUI.Show();
    }   
    public void HideGetHitUI()
    {
        getHitUI.Hide();
    }   
    public void ShowInventoryUI()
    {
        inventoryUI.Show();
    }
    public void HideInventoryUI()
    {
        inventoryUI.Hide();
    }
    public void ShowGrenadeUI()
    {
        grenadeUI.Show();
    }
    public void HideGrenadeUI()
    {
        grenadeUI.Hide();
    }   
    public void MainMenuBtnClicked()
    {
        HideEscMenu();
        HideInventoryUI();
        HideBulletUI();
        HideGrenadeUI();
        HideLevelNoUI();
        HideMessageUI();
        ShowMainMenu();
        HideTimer(.2f);
    }
    public void ExitMenuBtnClicked()
    {
        HideEscMenu();
        gameDirector.gameState = GameState.Playing;
    }
    public void ExitGameBtnClicked()
    {
        Application.Quit();
    }   

    public void StartGameButtonClicked()
    {
        if(gameDirector.levelManager.currentlevel == 1)
        {
            darkBG.gameObject.SetActive(true);  
            StartCoroutine(VideoPlayerCoroutine());
        }
        else
        {
            mainMenu.Hide();
            getHitUI.Show();
            ShowInventoryUI();
            gameDirector.RestartLevel();
        }
    }
    IEnumerator VideoPlayerCoroutine()
    {
        gameDirector.gameState = GameState.Video;
        mainMenu.Hide();
        videoPlayer.Play();
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        videoPlayer.Stop();
        gameDirector.gameState = GameState.Playing;
        darkBG.gameObject.SetActive(false);
        mainMenu.Hide();
        getHitUI.Show();
        ShowInventoryUI();
        gameDirector.RestartLevel();
    }
    public void NextLevelButtonClicked()
    {
        HideWinMenu();
        HideLoseMenu();
        ShowGetHitUI();
        gameDirector.LoadNextLevel();
    }
    public void PlayAgainButtonClicked()
    {
        HideWinMenu();
        HideLoseMenu();
        HideEscMenu();
        ShowGetHitUI();
        gameDirector.RestartLevel();
    }


}
