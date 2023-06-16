using FireballLockdownGame.ScoreRanking.Exceptions;
using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameInterfaceController : InterfaceBehavior
{
    public Text ScoreText;
    public Text OddText;
    public Text MiddleText;
    public Text FinalScoreText;
    public Text SpecialTimeCounter;
    public Text ErrorMessage;
    public InputField NicknameInputField;
    public InputField CodeInputField;
    public InputField PasswordInputField;
    public Button PlayAgainButton;
    public Button BackToMenuButton;
    [HideInInspector]
    public bool TimeCounterActivated = false;
    public AudioClip GameOverSoundEffect;
    public AudioClip BonusSoundEffect;
    public int ActualScore { get; private set; } = 0;
    public int Odd { get; set; } = 1;
    private bool _changesConfirmed = false;
    private bool _isGameOver = false;
    private ScoreRanking _scoreRanking;
    private PauseInterfaceController _pauseInterfaceController;
    private float _middleTextTimeCounter = 0;
    private float _timeCounter = 0;
    private float _maxTime = 0;
    private float _maxTimeCounter = 0;
    private string _pauseMessage = "PAUSADO";
    private string _gameOverMessage = "Game Over";
    private delegate void LastButtonPressed();
    private LastButtonPressed lastButtonPressed;

    void Awake()
    {
        ShowGameOverInterface(false, false);
        _scoreRanking = GetComponent<ScoreRanking>();
        _pauseInterfaceController = GameObject.FindWithTag(Tags.PauseInterface).GetComponent<PauseInterfaceController>();
        ScoreText.text = "Pontuação: " + ActualScore;
        ErrorMessage.gameObject.SetActive(false);
        MiddleText.gameObject.SetActive(false);
    }

    void Update()
    {
        MiddleTextController(_maxTime);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameStatus.IsPaused)
            {
                PauseGame(true);
            }
            else
            {
                PauseGame(false);
            }
        }
    }

    public void UpdateScore(int score)
    {
        ActualScore += score * Odd;
        ScoreText.text = "Pontuação: " + ActualScore;
    }

    public void ShowGameOverInterface(bool show, bool freezeGame)
    {
        if (freezeGame) Time.timeScale = 0;

        if (show)
        {
            GameOver(true);
        }
        else
        {
            GameOver(false);
        }
    }

    public void PauseGame(bool pause)
    {
        if (!_isGameOver)
        {
            _pauseInterfaceController.Pause(pause);
        }
    }

    public void RestartGameAndSaveScore()
    {
        lastButtonPressed = () => RestartGameAndSaveScore();
        bool saved = SaveScore();
        if (saved)
            StartGame();
    }

    public void BackToMenuAndSaveScore()
    {
        lastButtonPressed = () => BackToMenuAndSaveScore();
        bool saved = SaveScore();
        if (saved)
            BackToMenu();
    }

    public void ConfirmChanges()
    {
        SaveScore(true);
        ErrorMessage.gameObject.SetActive(false);
        _changesConfirmed = true;
        lastButtonPressed();
    }

    public GameInterfaceController ShowTemporaryMessage(string message, float time, bool timeCounter = false, bool soundEffect = false)
    {
        MiddleText.gameObject.SetActive(true);
        MiddleText.text = message;
        _maxTime = time;
        if (soundEffect)
        {
            AudioController.AudioSource.PlayOneShot(BonusSoundEffect);
        }

        return this;
    }

    public void SetTimeCounter(float time)
    {
        TimeCounterActivated = true;
        _maxTimeCounter = time;
    }

    public void HideErrorMessage()
    {
        ErrorMessage.gameObject.SetActive(false);
    }

    private void MiddleTextController(float maxTimeMessage)
    {
        if ((MiddleText.text != string.Empty && MiddleText.text != _gameOverMessage && MiddleText.text != _pauseMessage))
        {
            _middleTextTimeCounter += Time.deltaTime;

            if (_middleTextTimeCounter > maxTimeMessage)
            {
                MiddleText.gameObject.SetActive(false);
                _middleTextTimeCounter = 0;
            }

            _maxTimeCounter -= Time.deltaTime;
            if (TimeCounterActivated && _maxTimeCounter > 0)
            {
                SpecialTimeCounter.gameObject.SetActive(true);
                SpecialTimeCounter.text = _maxTimeCounter.ToString("F2");
            }
            else
            {
                SpecialTimeCounter.gameObject.SetActive(false);
                TimeCounterActivated = false;
                _timeCounter = 0;
            }
        }
    }

    private bool SaveScore(bool overwriteExistentData = false)
    {
        if (!string.IsNullOrEmpty(NicknameInputField.text))
        {
            ScoreModel scoreModel = new ScoreModel()
            {
                Score = ActualScore,
                Id = CodeInputField.text,
                Name = NicknameInputField.text,
                Password = PasswordInputField.text
            };

            bool userExists = _scoreRanking.UserExists(scoreModel.Id, scoreModel.Name);
            if (userExists)
            {
                bool passwordIsCorrect = _scoreRanking.CheckPassword(scoreModel.Id, scoreModel.Password);
                if (passwordIsCorrect)
                {
                    if (_scoreRanking.UserScore(scoreModel.Id) < scoreModel.Score)
                    {
                        _scoreRanking.SaveScoreOverwriting(scoreModel, x => x.Id ==  scoreModel.Id);
                        return true;
                    }
                }
                else
                {
                    ActiveErrorMessage($"Senha incorreta.\nO código '{scoreModel.Id}' e o nome '{scoreModel.Name}' já está sendo usado.\nEscolha outro código caso seja novo.");
                    return false;
                }
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(PasswordInputField.text))
                    {
                        ActiveErrorMessage("Adicione uma senha para seu usuário.");
                        return false;
                    }
                    _scoreRanking.SaveScore(scoreModel);
                    return true;
                }
                catch (SaveIdException e)
                {
                    ActiveErrorMessage(e.Message + "\nEscolha outro.");
                    return false;
                }
            }
        }

        if (!string.IsNullOrEmpty(CodeInputField.text) && string.IsNullOrEmpty(NicknameInputField.text))
        {
            ActiveErrorMessage("Adicione um nickname");
            return false;
        }

        return true;
    }

    private void GameOver(bool gameOver)
    {
        GameStatus.Pause(gameOver);
        _isGameOver = gameOver;
        if (gameOver)
        {
            AudioController.AudioSource.Stop();
            AudioController.AudioSource.PlayOneShot(GameOverSoundEffect);
            ScoreText.text = string.Empty;
            OddText.text = string.Empty;
            MiddleText.gameObject.SetActive(gameOver);
            MiddleText.text = _gameOverMessage;
            MiddleText.color = Color.red;
            FinalScoreText.text = "Sua pontuação: " + ActualScore;
            GameOverComponentsActive(gameOver);
        }
        else
        {
            MiddleText.text = string.Empty;
            MiddleText.color = Color.white;
            GameOverComponentsActive(gameOver);
        }
    }

    private void GameOverComponentsActive(bool visible)
    {
        CodeInputField.gameObject.SetActive(visible);
        NicknameInputField.gameObject.SetActive(visible);
        PasswordInputField.gameObject.SetActive(visible);
        PlayAgainButton.gameObject.SetActive(visible);
        BackToMenuButton.gameObject.SetActive(visible);
        SpecialTimeCounter.gameObject.SetActive(!visible);
        FinalScoreText.gameObject.SetActive(visible);
    }

    private void ActiveErrorMessage(string message)
    {
        ErrorMessage.gameObject.SetActive(true);
        ErrorMessage.text = message;
    }
}
