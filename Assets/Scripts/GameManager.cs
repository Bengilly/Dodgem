using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public AudioClip highscoreAudio;
    public AudioClip deathAudio;
    public AudioClip pickupAudio;

    private AudioSource audioSource;
    private VolumeSliderController volumeSlider;

    private int score;
    private int shopPoints;
    private int playerPoints;

    public GameState state;
    //public static event Action<GameState> OnGameStateChanged;

    public enum GameState
    {
        MainMenuScreen,
        GameScreen,
        ShopScreen,
        OptionsScreen,
        GameOverScreen
    }

    //apply singleton to gamemanager object
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Game Manager  is null");
            }

            return instance;
        }
        set
        {

        }
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //set initial state to main menu
        SetGameState(GameState.MainMenuScreen);
        CalculateShopPoints();

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        volumeSlider = GetComponent<VolumeSliderController>();
        audioSource.volume = volumeSlider.GetVolume();
    }

    //control screen state and load required UI elements
    public void SetGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.MainMenuScreen:
                ScreenManager.Instance.LoadMenuScreen();
                break;
            case GameState.GameScreen:
                ScreenManager.Instance.LoadGameScreen();
                StartGame();
                break;
            case GameState.ShopScreen:
                ScreenManager.Instance.LoadShopScreen();
                break;
            case GameState.OptionsScreen:
                ScreenManager.Instance.LoadOptionsScreen();
                break;
            case GameState.GameOverScreen:
                ScreenManager.Instance.LoadGameOverScreen();
                GameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        
        //OnGameStateChanged?.Invoke(newState);
    }

    //updates score value and displays it on the UI
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        ScreenManager.Instance.scoreGameUI.text = "Score: " + score;

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
    private void GameOver()
    {
        audioSource.PlayOneShot(deathAudio, 0.5f);
        CalculateShopPoints();
    }

    //calculate shop points by adding score to current points stored
    private void CalculateShopPoints()
    {
        playerPoints = (PlayerPrefs.GetInt("ShopPoints") + score);
        ScreenManager.Instance.shopPointsUI.text = "Shop Points: " + playerPoints;

        SetShopPoints(playerPoints);
    }

    public void SetShopPoints(int points)
    {
        shopPoints = points;
        ScreenManager.Instance.shopPointsUI.text = "Shop Points: " + shopPoints;
        PlayerPrefs.SetInt("ShopPoints", shopPoints);
    }

    public int GetShopPoints()
    {
        return shopPoints;
    }

    private void StartGame()
    {
        score = 0;
        UpdateScore(0);
        Time.timeScale = 1;
    }

    public void SaveButton()
    {
        volumeSlider.SaveVolume(volumeSlider.volumeValue);
    }

    public void QuitGame()
    {
        Instance = null;
        Destroy(gameObject);
        Application.Quit();
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void UpdateVolume()
    {
        audioSource.volume = volumeSlider.volumeValue / 10;
    }
}