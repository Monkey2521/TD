using UnityEngine;
using UnityEngine.UI;

public class TowerMaxLevelStat : MonoBehaviour
{
    [SerializeField] protected Text _statName;
    [SerializeField] protected Text _statLevel;
    [SerializeField] protected Text _statValue;

    public void Init(string statName, int statLevel, float statValue)
    {
        _statName.text = statName;
        _statLevel.text = "";
        _statValue.text = statValue.ToString();

        for (int i = 0; i < statLevel; i++)
            _statLevel.text += "*";
    }
}
