using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public bool priorityTarget = false;
    public int reward = 5;

    // Start is called before the first frame update
    void Start()
    {
        // Navigate to Castle
        GameObject castle = GameObject.Find("Castle");
        if (castle)
        {
            GetComponent<NavMeshAgent>().destination = castle.transform.position;
            GetComponentInChildren<Health>().maxHP = 100;
            
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Castle")
        {
            collider.GetComponentInChildren<Health>().TakeDamage(20);
            Destroy(gameObject);
        }
    }

    void OnMouseUpAsButton()
    {
        priorityTarget = true;
        Debug.Log("Selected target");
    }
}
