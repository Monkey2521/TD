using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    [Header("Debug settings")]
    [SerializeField] bool _isDebug;

    List<IMoveable> _moveables = new List<IMoveable>();

    static MoveSystem _instance;
    public static MoveSystem GetMoveSystem() => _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMoveable(IMoveable movable)
    {
        _moveables.Add(movable);
    }

    public void RemoveMoveable(IMoveable moveable)
    {
        if (_moveables.Contains(moveable))
            _moveables.Remove(moveable);
        else if (_isDebug) 
            Debug.Log("Missing moveable!");
    }

    void FixedUpdate()
    {
        foreach (IMoveable moveable in _moveables)
        {
            moveable.Move();
        }
    }
}
