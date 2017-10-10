using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    private int enemLvl;
    public int exp = 50;
    public float enemyDamage = 10;
    public float EnemyAttackRange = 3;

    void Start()
    {
        int minLvl = 9999;
        foreach (PlayerInventory temp in FindObjectsOfType<PlayerInventory>())
        {
            if(minLvl > temp.GetPlayerLvl())
            {
                minLvl = temp.GetPlayerLvl();
            }
        }
        enemLvl = minLvl;
        GetComponent<EnemyHealth>().SetEnemyHealth(enemLvl);
    }


    public int GetEnemyLvl()
    {
        return enemLvl;
    }

    public int ExpAmount()
    {
        return (int)(exp * Mathf.Pow(1.2f, enemLvl));
    }
}
