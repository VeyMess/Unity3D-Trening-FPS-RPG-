using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    private EnemyMove enemyMov;
    private int xp = 50;

    void Awake()
    {
        enemyMov = GetComponent<EnemyMove>();
        health = maxHealth;
    }

    public void TakeDamage(float dmgValue, Transform dmgDealear)
    {
        if (health > 0)
        {
            health -= dmgValue;
            if (health <= 0f)
            {
                enemyMov.isAlive = false;
                dmgDealear.GetComponent<PlayerInventory>().GiveEXP(GetComponent<EnemyStats>().ExpAmount());
                GetComponent<EnemyDrop>().SpawnDrop();
                if(GetComponentInParent<BasicSpawn>() != null)
                {
                    GetComponentInParent<BasicSpawn>().EnemyKilled();
                }
            }
        }
        enemyMov.SetAgro(dmgDealear);
        enemyMov.AnimHit();
    }

    public float ReturnHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetEnemyHealth(int lvl)
    {
        if (lvl != 1)
        {
            maxHealth = 100 * Mathf.Pow(1.5f, lvl);
            health = maxHealth;
        }
    }
}
