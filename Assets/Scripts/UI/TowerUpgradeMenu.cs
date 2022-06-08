using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Text _towerName;
    [SerializeField] Image _towerIcon;

    [Header("Upgrade menu")]
    [SerializeField] GameObject _upgradeMenu;
    [SerializeField] Text _upgradeCost;

    [SerializeField] TowerUpgradeStat _upgradeAttackRange;
    [SerializeField] TowerUpgradeStat _upgradeAttackTime;
    [SerializeField] TowerUpgradeStat _upgradeDamage;

    [Header("Max level menu")]
    [SerializeField] GameObject _maxLevelMenu;
    [SerializeField] TowerMaxLevelStat _attackRange;
    [SerializeField] TowerMaxLevelStat _attackTime;
    [SerializeField] TowerMaxLevelStat _damage;

    Tower _tower;

    public void Init(Tower tower)
    {
        _tower = tower;
        _upgradeMenu.SetActive(false);
        _maxLevelMenu.SetActive(false);

        _towerName.text = _tower.Name;
        _towerIcon.sprite = _tower.Icon;
        
        if (_tower.Level == _tower.MaxLevel)
        {
            _maxLevelMenu.SetActive(true);
            _attackRange.Init("Attack range", _tower.Level, _tower.AttackRange);
            _attackTime.Init("Attack time", _tower.Level, _tower.AttackTime);
            _damage.Init("Damage", _tower.Level, _tower.Damage);
        }

        else
        {
            _upgradeMenu.SetActive(true);
            _upgradeCost.text = "UPGRADE (" + _tower.UpgradeCost.ToString() + ")";

            _upgradeAttackRange.Init("Attack range", _tower.Level, _tower.AttackRange, _tower.UpperAttackRange);
            _upgradeAttackTime.Init("Attack time", _tower.Level, _tower.AttackTime, _tower.UpperAttackTime);
            _upgradeDamage.Init("Damage", _tower.Level, _tower.Damage, _tower.UpperDamage);
        }
    }

    public void Upgrade()
    {
        if (_tower.Upgrade())
        {
            Init(_tower);
        }
        else
        {
            Debug.Log("Can`t upgrade tower!");
        }
    }
}
