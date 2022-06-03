using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int currentGold;

    private void Awake()
    {
        GetComponentInChildren<Health>().maxHP = 100;
        currentGold = 50;
        Debug.LogWarning("Current gold = " + currentGold);
    }

    void OnMouseUpAsButton()
    {
        Debug.LogWarning("Current gold = " + currentGold);
    }
}
