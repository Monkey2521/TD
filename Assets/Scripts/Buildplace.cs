using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildplace : MonoBehaviour
{
    private bool empty = true;
    private int buildCost = 20;
    
    public GameObject towerPrefab;

    private GameObject castle;

    void Start()
    {
        castle = GameObject.Find("Castle");
    }

    void OnMouseUpAsButton()
    {
        
        if (empty == true && castle.GetComponent<Castle>().currentGold >= buildCost)
        {
            Debug.Log("Build tower");

            BuildTower();

            empty = false;
        }
        else Debug.Log("FUCK YOU!!!");
    }

    private void BuildTower()
    {
        castle.GetComponent<Castle>().currentGold -= buildCost;
        GameObject tower = (GameObject)Instantiate(towerPrefab);
        tower.transform.position = transform.position + Vector3.up;
    }
}
