using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpawn : MonoBehaviour
{
    public List<GameObject> enemys;
    public List<GameObject> spawnPoints;

    public float respawnTime = 10f;

    private float timePass = 0;
    private bool isSpawning= false;
    private int killCount=0;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        int spwIn = 0;
        foreach (GameObject target in enemys)
        {
            Instantiate(target, spawnPoints[spwIn].transform.position, spawnPoints[spwIn].transform.rotation, transform);
            spwIn++;
        }
    }

    public void EnemyKilled()
    {
        killCount++;

        if(killCount == enemys.Count)
        {
            isSpawning = true;
            killCount = 0;
        }
    }

    void Update()
    {
        if(isSpawning)
        {
            if (timePass >= respawnTime)
            {
                isSpawning = false;
                timePass = 0;
                Spawn();
            }
            else
                timePass += Time.deltaTime;
        }
    }
}
