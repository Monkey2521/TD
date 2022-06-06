using UnityEngine;
using UnityEngine.UI;

public class TowerBuildPreview : MonoBehaviour
{
    [SerializeField] Text _cost;
    [SerializeField] Text _damage;
    [SerializeField] Text _attackRange;
    [SerializeField] Text _attackTime;

    public void SetText(int cost, int damage, float range, float time)
    {
        _cost.text = cost.ToString();
        _damage.text = damage.ToString();
        _attackRange.text = range.ToString();
        _attackTime.text = time.ToString();
    }
}
