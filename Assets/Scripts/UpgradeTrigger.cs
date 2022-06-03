using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTrigger : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        transform.parent.SendMessageUpwards("Upgrade");
    }
}
