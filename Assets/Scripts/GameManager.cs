using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI highscoreTextGameOver;
    public GameObject gameScreen;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject optionsScreen;
    public AudioClip highscoreAudio;
    public AudioClip deathAudio;
    public AudioClip pickupAudio;
    //public AudioClip gameMusic;
    public bool isGameActive = false;

    public AudioSource audioSource;
    private VolumeSliderController volumeSlider;
    private Button startButton;
    private Button restartButton;
    private Button optionsButton;
    private Button returnButton;
    private Button resetButton;
    private Button saveButton;
    private Button quitButton;
    private int score;


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
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volumeSlider.volumeValue/10;
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
        gameOverScreen.gameObject.SetActive(true);
        isGameActive = false;

        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(RestartGame);
        highscoreTextGameOver.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();
    }

    public void StartGame()
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

        returnButton = GameObject.Find("ReturnButton").GetComponent<Button>();
        returnButton.onClick.AddListener(TitleScreen);

       saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
       saveButton.onClick.AddListener(SaveButton);

    }

    //load title screen
    private void TitleScreen()
    {
        titleScreen.gameObject.SetActive(true);
        optionsScreen.gameObject.SetActive(false);
    }
    
    private void SaveButton()
    {
        volumeSlider.SaveVolume(volumeSlider.volumeValue);
        audioSource.volume = volumeSlider.GetVolume();
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteKey("Highscore");
        highscoreText.text = "0";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
