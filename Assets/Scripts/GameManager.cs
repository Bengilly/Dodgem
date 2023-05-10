using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI highscoreTextGameOver;
    public TextMeshProUGUI shopPoints;
    public GameObject gameScreen;
    public GameObject shopScreen;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject optionsScreen;
    public AudioClip highscoreAudio;
    public AudioClip deathAudio;
    public AudioClip pickupAudio;
    //public AudioClip gameMusic;
    public bool isGameActive = false;

    private AudioSource audioSource;
    private VolumeSliderController volumeSlider;
    private Button startButton;
    private Button shopButton;
    private Button restartButton;
    private Button optionsButton;
    private Button returnButton;
    private Button resetButton;
    private Button saveButton;
    private Button quitButton;
    private int score;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        titleScreen.gameObject.SetActive(true);

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
        Debug.Log(audioSource.volume);

        shopButton = GameObject.Find("ShopButton").GetComponent<Button>();
        shopButton.onClick.AddListener(StoreScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //updates score value and displays it on the UI
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;

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

    //when the player dies, end game and display gameover UI
    public void GameOver()
    {
        audioSource.PlayOneShot(deathAudio, 0.5f);
        gameScreen.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(true);
        isGameActive = false;

        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(RestartGame);
        highscoreTextGameOver.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();

        CalculateShopPoints();
    }

    //calculate shop points by adding score to current points stored
    private void CalculateShopPoints()
    {
        int playerPoints;
        playerPoints = (PlayerPrefs.GetInt("ShopPoints", 0) + score);
        shopPoints.text = "Shop Points: " + playerPoints;
        PlayerPrefs.SetInt("ShopPoints", playerPoints);
    }

    private void StartGame()
    {

        gameScreen.gameObject.SetActive(true);
        titleScreen.gameObject.SetActive(false);

        highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();
        isGameActive = true;
        score = 0;
        UpdateScore(0);
        Time.timeScale = 1;
    }

    //load options menu
    private void OptionsMenu()
    {
        optionsScreen.gameObject.SetActive(true);
        titleScreen.gameObject.SetActive(false);

        resetButton = GameObject.Find("ResetHighscores").GetComponent<Button>();
        resetButton.onClick.AddListener(ResetScore);

        returnButton = GameObject.Find("OptionsReturnButton").GetComponent<Button>();
        returnButton.onClick.AddListener(TitleScreen);

       saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
       saveButton.onClick.AddListener(SaveButton); 
    }

    //load title screen
    private void TitleScreen()
    {
        titleScreen.gameObject.SetActive(true);
        optionsScreen.gameObject.SetActive(false);
        shopScreen.gameObject.SetActive(false);
    }

    private void StoreScreen()
    {
        shopScreen.gameObject.SetActive(true);
        titleScreen.gameObject.SetActive(false);

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
        highscoreText.text = "0";
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