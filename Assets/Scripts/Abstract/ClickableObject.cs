using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    [Header("Debug settings")]
    [SerializeField] protected bool _isDebug;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (_isDebug) Debug.Log("Click on " + name);
    }
}
