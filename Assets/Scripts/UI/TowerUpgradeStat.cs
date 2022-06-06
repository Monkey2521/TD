using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeStat : TowerMaxLevelStat
{
    [SerializeField] protected Text _statUpperLevel;
    [SerializeField] protected Text _statUpperValue;

    public void Init(string statName, int statLevel, float statValue, float statUpperValue)
    {
        base.Init(statName, statLevel, statValue);

        _statUpperLevel.text = "";
        _statUpperValue.text = statUpperValue.ToString();

        for (int i = 0; i < statLevel + 1; i++)
            _statUpperLevel.text += "*";
    }
}
