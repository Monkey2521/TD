using UnityEngine;
using UnityEngine.UI;

public class TowerBuildPreview : MonoBehaviour
{
    [SerializeField] Text _cost;
    [SerializeField] Text _damage;
    [SerializeField] Text _attackRange;
    [SerializeField] Text _attackTime;
    [SerializeField] Image _icon;

    Tower _tower;
    BuildPanel _builder;

    public void Init(Tower tower, BuildPanel builder)
    {
        _cost.text = tower.BuildCost.ToString();
        _damage.text = tower.Damage.ToString();
        _attackRange.text = tower.AttackRange.ToString();
        _attackTime.text = tower.AttackTime.ToString();
        _icon.sprite = tower.Icon;

        _tower = tower;
        _builder = builder;
    }

    public void Build()
    {
        _builder.Build(_tower);
    }

    public void ShowRange()
    {
        _builder.ShowAttackRange(_tower);
    }
}
