using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region public
    public GameObject bulletPrefab;
    public float rotationSpeed = 30, attackSpeed = 3;
    public int bulletDamage = 10, level = 1;
    #endregion

    #region private
    private bool attackPriority = false;
    private int maxLevel = 5, enemyID = 0, upgradeCost = 25;
    private float wait;
    private GameObject castle;
    #endregion

    void Start()
    {
        castle = GameObject.Find("Castle");
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Enter enemy with ID: " + collider.GetInstanceID());
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.GetComponent<Monster>() && !attackPriority && collider.name != "Castle" &&
            collider.GetComponent<Monster>().priorityTarget == true)
        {
            StopAllCoroutines();
            enemyID = collider.GetInstanceID();
            attackPriority = true;
            StartCoroutine(AttackTarget(collider));
        }
        else if (collider.GetComponent<Monster>() && enemyID == 0)
        {
            enemyID = collider.GetInstanceID();
            Debug.Log("Enemy ID = " + enemyID);
            StartCoroutine(AttackTarget(collider));
        }
    }

    void OnTriggerExit(Collider collider)
    {
        Debug.Log("Exit enemy with ID: " + collider.GetInstanceID());

        if (collider.GetInstanceID() == enemyID)
        {
            Debug.Log("PIDARAS");
            attackPriority = false;
            enemyID = 0;
            StopAllCoroutines();
        }
        else Debug.Log("Don't change target");
    }
    
    IEnumerator WaitReload()
    {
        yield return new WaitForSeconds(Time.time - wait);
    }

    IEnumerator AttackTarget(Collider collider)
    {
        while (enemyID != 0 && collider != null)
        {
            Debug.Log("Attack enemy with ID = " + enemyID);

            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponentInChildren<Bullet>().damage = bulletDamage;
            bullet.GetComponent<Bullet>().target = collider.transform;

            Debug.Log("Reload");
            wait = Time.time;
            yield return new WaitForSeconds(attackSpeed);
        }

        Debug.Log("Target destroyed");
        enemyID = 0;
        attackPriority = false;
    }

    public void Upgrade()
    {
        if (level < maxLevel && castle.GetComponent<Castle>().currentGold >= upgradeCost * level)
        {
            castle.GetComponent<Castle>().currentGold -= upgradeCost;
            Debug.Log("Upgrade tower");

            SphereCollider sphere = GetComponent<SphereCollider>();
            sphere.radius += 1f;

            attackSpeed -= 0.5f;
            bulletDamage += 5;

            level++;
        }
        else if (level == maxLevel) Debug.Log("Max level!");
        else Debug.LogWarning("Not enough gold for upgrade!");
    }
}
