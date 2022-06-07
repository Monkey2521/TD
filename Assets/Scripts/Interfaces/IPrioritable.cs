using UnityEngine;

public interface IPrioritable
{
    public bool IsPriorityTarget { get; set; }

    public void SetPriority();
}
