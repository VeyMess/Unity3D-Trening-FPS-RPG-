using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    private float health = 100f;
    private float maxHealth = 100f;
    private bool dead = false;

    public Slider playerHelathSlider;


    //Picked Health
    public void HealthChange(float chage)
    {
        if ((health + chage) < maxHealth)
        {
            health += chage;
        }
        else
            health = maxHealth;

        playerHelathSlider.value = health;

        if (health <= 0f)
            dead = true;
    }

    //Picked Damage
    public void MaxHealthChange(float chage)
    {
        maxHealth += chage;
        playerHelathSlider.maxValue = maxHealth;
    }

    public float GetPlayerHealth()
    {
        return health;
    }

    public float GetPlayerMaxHP()
    {
        return maxHealth;
    }
}
