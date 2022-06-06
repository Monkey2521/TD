using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text _scoreText;
    int _score;

    EventManager _eventManager;

    void Start()
    {
        _eventManager = EventManager.GetEventManager();
        _eventManager.OnEnemyKilled.AddListener(UpdateCounter);
        _eventManager.OnGameStart.AddListener(Restart);
    }

    void UpdateCounter(EnemyController enemy)
    {
        _score += enemy.ScorePoints;
        _scoreText.text = _score.ToString(); 
    }

    void Restart()
    {
        _score = 0;
    }
}
