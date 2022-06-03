using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    EventManager _eventManager;

    void Start ()
    {
        _eventManager = EventManager.GetEventManager();
    }

    [ContextMenu("Restart")]
    public void Restart ()
    {
        _eventManager.OnGameStart?.Invoke();
    }
}
