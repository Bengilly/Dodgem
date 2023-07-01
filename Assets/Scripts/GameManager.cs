﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public TextMeshProUGUI scoreGameUI;
    public TextMeshProUGUI highscoreGameUI;
    public TextMeshProUGUI gameOverTextGameOverUI;
    public TextMeshProUGUI highscoreGameOverUI;
    public TextMeshProUGUI shopPointsUI;
    public GameObject gameScreen;
    public GameObject shopScreen;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject optionsScreen;
    public AudioClip highscoreAudio;
    public AudioClip deathAudio;
    public AudioClip pickupAudio;


    private AudioSource audioSource;
    private VolumeSliderController volumeSlider;
    private Button startButton;
    private Button shopButton;
    private Button restartButton;
    private Button optionsButton;
    private Button returnButton;
    private Button highscoreResetButton;
    private Button shopPointsResetButton;
    private Button saveButton;
    private Button quitButton;
    private int score;
    private int shopPoints;
    private int playerPoints;
    private bool isGameActive = false;

    //private ScreenState currentState;

    //future work - implement state switching for game screens
    private enum ScreenState
    {
        MainMenuScreen,
        GameScreen,
        ShopScreen,
        OptionsScreen,
        GameOverScreen
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager  is null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //currentState = ScreenState.MainMenuScreen;
        //SetGameState(currentState);

        titleScreen.SetActive(true);

        startButton = GameObject.Find("PlayButton").GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);

        optionsButton = GameObject.Find("OptionsButton").GetComponent<Button>();
        optionsButton.onClick.AddListener(OptionsMenu);

        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        quitButton.onClick.AddListener(QuitGame);

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        volumeSlider = GetComponent<VolumeSliderController>();
        audioSource.volume = volumeSlider.GetVolume();

        shopButton = GameObject.Find("ShopButton").GetComponent<Button>();
        shopButton.onClick.AddListener(StoreScreen);

        CalculateShopPoints();
    }

    //private void SetGameState(ScreenState state)
    //{
    //    currentState = state;

    //    switch (state)
    //    {
    //        default:
    //        case ScreenState.MainMenuScreen:
    //            LoadMenuState(); break;
    //        case ScreenState.GameScreen:
    //            LoadGameState(); break;
    //        case ScreenState.ShopScreen:
    //            LoadShopState(); break;
    //        case ScreenState.OptionsScreen:
    //            LoadOptionsState(); break;
    //        case ScreenState.GameOverScreen:
    //            LoadGameOverState(); break;
    //    }
    //} 
    //private void LoadMenuState()
    //{

    //}
    //private void LoadGameState()
    //{

    //}
    //private void LoadShopState()
    //{

    //}
    //private void LoadOptionsState()
    //{

    //}
    //private void LoadGameOverState()
    //{

    //}

    //updates score value and displays it on the UI
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreGameUI.text = "Score: " + score;

        if (score <= PlayerPrefs.GetInt("Highscore", 0))
        {
            audioSource.PlayOneShot(pickupAudio, 0.3f);
        }
        else if (score > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", score);
            audioSource.PlayOneShot(highscoreAudio, 0.3f);
        }

    }

    public bool IsGameOver()
    {
        return isGameActive;
    }

    //when the player dies, end game and display gameover UI
    public void GameOver()
    {
        audioSource.PlayOneShot(deathAudio, 0.5f);
        gameScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        isGameActive = false;

        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(RestartGame);

        string highscore = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();
        highscoreGameUI.text = highscore;
        highscoreGameOverUI.text = highscore;
        
        CalculateShopPoints();
    }

    //calculate shop points by adding score to current points stored
    private void CalculateShopPoints()
    {
        playerPoints = (PlayerPrefs.GetInt("ShopPoints") + score);
        shopPointsUI.text = "Shop Points: " + playerPoints;

        SetShopPoints(playerPoints);
    }

    public void SetShopPoints(int shopPoints)
    {
        this.shopPoints = shopPoints;
        shopPointsUI.text = "Shop Points: " + shopPoints;
        PlayerPrefs.SetInt("ShopPoints", shopPoints);
        Debug.Log(shopPoints);
    }

    public int GetShopPoints()
    {
        return shopPoints;
    }

    private void StartGame()
    {
        gameScreen.SetActive(true);
        titleScreen.SetActive(false);

        highscoreGameUI.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();

        isGameActive = true;
        score = 0;
        UpdateScore(0);
        Time.timeScale = 1;
    }

    //load options menu
    private void OptionsMenu()
    {
        optionsScreen.SetActive(true);
        titleScreen.SetActive(false);

        highscoreResetButton = GameObject.Find("ResetHighscores").GetComponent<Button>();
        highscoreResetButton.onClick.AddListener(ResetScore);

        shopPointsResetButton = GameObject.Find("ResetShopPoints").GetComponent<Button>();
        shopPointsResetButton.onClick.AddListener(ResetShopPoints);
        

        returnButton = GameObject.Find("OptionsReturnButton").GetComponent<Button>();
        returnButton.onClick.AddListener(TitleScreen);

        saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
        saveButton.onClick.AddListener(SaveButton); 
    }

    //load title screen
    private void TitleScreen()
    {
        titleScreen.SetActive(true);
        optionsScreen.SetActive(false);
        shopScreen.SetActive(false);
    }

    private void StoreScreen()
    {
        shopScreen.SetActive(true);
        titleScreen.SetActive(false);

        shopPointsUI.text = "Shop Points: " + PlayerPrefs.GetInt("ShopPoints", 0).ToString();

        returnButton = GameObject.Find("ShopReturnButton").GetComponent<Button>();
        returnButton.onClick.AddListener(TitleScreen);
    }
    
    private void SaveButton()
    {
        volumeSlider.SaveVolume(volumeSlider.volumeValue);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void ResetScore()
    {
        PlayerPrefs.DeleteKey("Highscore");
        highscoreGameUI.text = "0";
    }

    private void ResetShopPoints()
    {
        PlayerPrefs.DeleteKey("ShopPoints");
        shopPointsUI.text = "0";
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void UpdateVolume()
    {
        audioSource.volume = volumeSlider.volumeValue / 10;
    }
}