using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Debug settings")]
    [SerializeField] bool _isDebug;

    [Header("Settings")]
    [SerializeField] TextMesh _healthText;
    [SerializeField] Color _color;

    [SerializeField] char _symbol;
    [SerializeField] int _symbCount;

    [SerializeField] IDamageable _parent;

    public void Init(IDamageable parent)
    {
        _parent = parent;

        _healthText.color = _color;

        UpdateHealth();
    }

    public void UpdateHealth()
    {
        _healthText.text = "";

        float multiplier = (float)_parent.HP / (float)_parent.MaxHP;

        for (int i = 0; i < (int)(_symbCount * multiplier); i++)
            _healthText.text += _symbol;
    }
}
