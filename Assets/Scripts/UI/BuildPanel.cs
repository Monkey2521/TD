using UnityEngine;

public class BuildPanel : MonoBehaviour
{
    [Header("Debug settings")]
    [SerializeField] bool _isDebug;

    [Header("Settings")]
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _upgradeMenu;
    [SerializeField] GameObject _buildings;

    Buildplace _buildplace;
    
    EventManager _eventManager;

    void Start()
    {
        _eventManager = EventManager.GetEventManager();

        _eventManager.OnBuildplaceClick.AddListener(Init);
        _eventManager.OnGameOver.AddListener(HideBuilds);
    }

    void Init(Buildplace buildplace)
    {
        if (_isDebug) Debug.Log("Init");

        _buildplace = buildplace;

        if (_buildplace.IsEmpty)
        {
            if (!_animator.GetBool("Show"))
            {
                ShowBuilds();
            }

            ShowBuildings();
        }
        else
        {
            ShowUpgradeMenu(_buildplace.Tower);
        }
    }

    void ShowBuilds()
    {
        _animator.SetBool("Hide", false);
        _animator.SetBool("Show", true);
    }

    public void HideBuilds()
    {
        _animator.SetBool("Hide", true);
        _animator.SetBool("Show", false);

        _buildings.gameObject.SetActive(false);
        _upgradeMenu.gameObject.SetActive(false);

        _buildplace = null;
    }

    void ShowBuildings()
    {
        _buildings.gameObject.SetActive(true);
    }

    void ShowUpgradeMenu(Tower tower)
    {
        _upgradeMenu.gameObject.SetActive(true);
    }
}
