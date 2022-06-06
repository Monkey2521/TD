using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;

    [SerializeField] GameObject _HUD;

    [SerializeField] ScoreCounter _scoreCounter;
    [SerializeField] GameObject _gameOverMenu;
    [SerializeField] UnityEngine.UI.Text _totalScore;

    [SerializeField] GameObject _pauseMenu;

    static bool _isPaused;
    public static bool IsPaused => _isPaused;

    EventManager _eventManager;

    void Start ()
    {
        _eventManager = EventManager.GetEventManager();

        _eventManager.OnGameOver.AddListener(ShowGameOver);
    }

    [ContextMenu("Restart")]
    public void Restart ()
    {
        _HUD.SetActive(true);
        _gameOverMenu.SetActive(false);
        _pauseMenu.SetActive(false);
        _mainMenu.SetActive(false);

        _eventManager.OnGameStart?.Invoke();
    }

    void ShowGameOver()
    {
        _gameOverMenu.SetActive(true);
        _totalScore.text = "Total score: " + _scoreCounter.Score;
        
        _HUD.SetActive(false);
        _pauseMenu.SetActive(false);
        _mainMenu.SetActive(false);
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        _pauseMenu.SetActive(_isPaused);

        _HUD.SetActive(!_isPaused);
        _mainMenu.SetActive(!_isPaused);
        _gameOverMenu.SetActive(!_isPaused);
    }

    public void ShowMainMenu()
    {
        _mainMenu.SetActive(true);

        _HUD.SetActive(false);
        _pauseMenu.SetActive(false);
        _gameOverMenu.SetActive(false);
    } 

    public void ExitGame()
    {
        Application.Quit();
    }
}
