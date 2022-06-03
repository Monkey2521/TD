using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP;

    private int sumDMG = 0, maxLen;
    private GameObject castle;

    TextMesh health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<TextMesh>();
        maxLen = Current();
        castle = GameObject.Find("Castle");
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public int Current()
    {
        return health.text.Length;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("SOSAT GAD!!! damage = " + damage);
        sumDMG += damage;
        if (sumDMG >= maxHP / maxLen * (maxLen - Current() + 1))
            Decrease();
    }

    private void Decrease()
    {
        Debug.Log("Current HP = " + (maxHP - sumDMG));
        if (Current() > 1)
            health.text = health.text.Remove(health.text.Length - 1);
        else
        {
            if (transform.parent.name != "Castle")
            {
                castle.GetComponent<Castle>().currentGold += transform.parent.GetComponent<Monster>().reward;
                Debug.LogWarning("- PIDARAS!!! + " + transform.parent.GetComponent<Monster>().reward + " gold");
            }
            Destroy(transform.parent.gameObject);
        }
            
    }
}
