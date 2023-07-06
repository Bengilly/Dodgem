using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;

    public TextMeshProUGUI scoreGameUI;
    public TextMeshProUGUI highscoreGameUI;
    public TextMeshProUGUI gameOverTextGameOverUI;
    public TextMeshProUGUI highscoreGameOverUI;
    public TextMeshProUGUI shopPointsUI;
    public TextMeshProUGUI textPopup;

    private Button startButton;
    private Button shopButton;
    private Button restartButton;
    private Button optionsButton;
    private Button shopReturnButton;
    private Button optionsReturnButton;
    private Button highscoreResetButton;
    private Button shopPointsResetButton;
    private Button saveButton;
    private Button quitButton;

    private Vector3 textPopupPosition = new Vector3(-58f, -250f, 0f);

    public GameObject titleScreen;
    public GameObject gameScreen;
    public GameObject shopScreen;
    public GameObject optionsScreen;
    public GameObject gameOverScreen;

    //apply singleton to screenmanager object
    public static ScreenManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Screen Manager  is null");
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    //load menu ui
    public void LoadMenuScreen()
    {
        titleScreen.SetActive(true);
        optionsScreen.SetActive(false);
        shopScreen.SetActive(false);

        startButton = GameObject.Find("PlayButton").GetComponent<Button>();
        startButton.onClick.AddListener(() => GameManager.Instance.SetGameState(GameManager.GameState.GameScreen));

        shopButton = GameObject.Find("ShopButton").GetComponent<Button>();
        shopButton.onClick.AddListener(() => GameManager.Instance.SetGameState(GameManager.GameState.ShopScreen));

        optionsButton = GameObject.Find("OptionsButton").GetComponent<Button>();
        optionsButton.onClick.AddListener(() => GameManager.Instance.SetGameState(GameManager.GameState.OptionsScreen));

        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        quitButton.onClick.AddListener(GameManager.Instance.QuitGame);
    }

    //load game ui
    public void LoadGameScreen()
    {
        titleScreen.SetActive(false);
        gameScreen.SetActive(true);

        highscoreGameUI.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();
    }

    //load shop ui
    public void LoadShopScreen()
    {
        titleScreen.SetActive(false);
        shopScreen.SetActive(true);

        shopReturnButton = GameObject.Find("ShopReturnButton").GetComponent<Button>();
        shopReturnButton.onClick.AddListener(() => GameManager.Instance.SetGameState(GameManager.GameState.MainMenuScreen));

        shopPointsUI.text = "Shop Points: " + PlayerPrefs.GetInt("ShopPoints", 0).ToString();
    }

    //load options ui
    public void LoadOptionsScreen()
    {
        titleScreen.SetActive(false);
        optionsScreen.SetActive(true);

        highscoreResetButton = GameObject.Find("ResetHighscores").GetComponent<Button>();
        highscoreResetButton.onClick.AddListener(ResetScore);

        shopPointsResetButton = GameObject.Find("ResetShopPoints").GetComponent<Button>();
        shopPointsResetButton.onClick.AddListener(ResetShopPoints);

        optionsReturnButton = GameObject.Find("OptionsReturnButton").GetComponent<Button>();
        optionsReturnButton.onClick.AddListener(() => GameManager.Instance.SetGameState(GameManager.GameState.MainMenuScreen));

        saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
        saveButton.onClick.AddListener(GameManager.Instance.SaveButton);
    }

    //load gameover ui
    public void LoadGameOverScreen()
    {
        titleScreen.SetActive(false);
        gameOverScreen.SetActive(true);

        string highscore = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();
        highscoreGameUI.text = highscore;
        highscoreGameOverUI.text = highscore;

        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
    }

    private void ResetScore()
    {
        PlayerPrefs.DeleteKey("Highscore");
        highscoreGameUI.text = "0";
        Debug.Log("Highscore reset");

        StartCoroutine(ShowMessageLog("Highscores have been reset!", textPopupPosition, 3));
    }

    private void ResetShopPoints()
    {
        PlayerPrefs.DeleteKey("ShopPoints");
        shopPointsUI.text = "0";
        Debug.Log("Shop points reset");
        StartCoroutine(ShowMessageLog("Shop points have been reset!", textPopupPosition, 3));
    }

    public void ShowCharacterPopupText(string character, int points)
    {
        
        StartCoroutine(ShowMessageLog($"Purchased {character} for {points} points.", textPopupPosition, 3));
    }

    IEnumerator ShowMessageLog(string message, Vector3 position, float delay)
    {
        textPopup.transform.localPosition = position;
        textPopup.text = message;
        textPopup.enabled = true;
        yield return new WaitForSeconds(delay);
        textPopup.enabled = false;
    }
}

