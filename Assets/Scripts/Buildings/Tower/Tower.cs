using UnityEngine;

public class Tower : ClickableObject
{
    [Header("Settings")]
    [SerializeField] int _cost;
    public int Cost => _cost;
}
