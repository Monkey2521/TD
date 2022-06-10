using System.Collections.Generic;
using UnityEngine;

public class BuildPanel : MonoBehaviour
{
    [Header("Debug settings")]
    [SerializeField] bool _isDebug;

    [Header("Settings")]
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _selectedLinePrefab;
    GameObject _line;

    [Header("UpgradeMenu")]
    [SerializeField] TowerUpgradeMenu _upgradeMenu;

    [Header("Buildings")]
    [SerializeField] GameObject _buildings;
    [SerializeField] Transform _previewParent;

    [SerializeField] List<Tower> _towers = new List<Tower>();
    [SerializeField] TowerBuildPreview _previewPrefab;
    List<TowerBuildPreview> _previewList = new List<TowerBuildPreview>();

    Buildplace _buildplace;
    
    EventManager _eventManager;

    void Start()
    {
        _eventManager = EventManager.GetEventManager();

        _eventManager.OnBuildplaceClick.AddListener(Init);
        _eventManager.OnTowerClick.AddListener(Init);
        _eventManager.OnGameOver.AddListener(HideBuilds);
        
        _line = Instantiate(_selectedLinePrefab);
        _line.SetActive(false);
    }

    void Init(Buildplace buildplace)
    {
        Init(buildplace, null);
        _line.transform.position = buildplace.transform.position + Vector3.up * 0.6f;
    }

    void Init(Tower tower)
    {
        Init(null, tower);
        _line.transform.position = tower.Buildplace.transform.position + Vector3.up * 0.6f;
    }

    void Init(Buildplace buildplace, Tower tower)
    {
        if (_isDebug) Debug.Log("Init");

        _line.SetActive(true);

        if (!_animator.GetBool("Show"))
        {
            ShowBuilds();
        }

        if (buildplace != null)
        {
            _buildplace = buildplace;

            if (_buildplace.IsEmpty)
            {
                ShowBuildings();
            }
            else
            {
                ShowUpgradeMenu(_buildplace.Tower);
            }
        }
        else if (tower != null)
        {
            ShowUpgradeMenu(tower);
        }

    }

    void ShowBuilds()
    {
        _animator.SetBool("Hide", false);
        _animator.SetBool("Show", true);
    }

    public void HideBuilds()
    {
        _line.SetActive(false);
        _animator.SetBool("Hide", true);
        _animator.SetBool("Show", false);

        _buildplace.ChangeTowerRange();
        _buildplace = null;
    }

    public void DeactivateBuildPanels()
    {
        _buildings.gameObject.SetActive(false);
        _upgradeMenu.gameObject.SetActive(false);
    }

    void ShowBuildings()
    {
        _upgradeMenu.gameObject.SetActive(false);
        _buildings.gameObject.SetActive(true);

        if (_previewList.Count < _towers.Count)
        {
            while(_previewList.Count > 0)
            {
                Destroy(_previewList[0].gameObject);
                _previewList.Remove(_previewList[0]);
            }

            foreach(Tower tower in _towers)
            {
                TowerBuildPreview preview = Instantiate(_previewPrefab, _previewParent);
                preview.Init(tower, this);

                _previewList.Add(preview);
            }
        }
        
    }

    void ShowUpgradeMenu(Tower tower)
    {
        _buildings.gameObject.SetActive(false);
        _upgradeMenu.gameObject.SetActive(true);

        _upgradeMenu.Init(tower);
    }

    public void Build(Tower tower)
    {
        if (_buildplace != null && _buildplace.IsEmpty)
        {
            if (_buildplace.Build(tower))
                ShowUpgradeMenu(_buildplace.Tower);
            else if (_isDebug) Debug.Log("Cant build " + tower.Name);
        }
    }

    public void ShowRangePreview(Tower tower)
    {
        if (_buildplace != null)
        {
            
        }
    }
}
