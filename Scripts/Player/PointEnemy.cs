using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointEnemy : MonoBehaviour {

    public Slider enemyHealthSlider;
    public Image imgOfEnemyHealth;
    public Text enemLvlName;

	void Update ()
    {
        RaycastHit rayHit;
        

        if(Physics.Raycast(transform.position,transform.forward, out rayHit))
        {
            if (rayHit.collider.tag.Equals("Enemy"))
            {
                EnemyHealth temp = rayHit.collider.GetComponentInParent<EnemyHealth>();
                imgOfEnemyHealth.enabled = true;
                enemLvlName.enabled = true;
                enemyHealthSlider.maxValue = temp.GetMaxHealth();
                enemyHealthSlider.value = temp.ReturnHealth();
                enemLvlName.text = temp.GetComponent<EnemyStats>().GetEnemyLvl() + " LvL";
            }
            else
            {
                imgOfEnemyHealth.enabled = false;
                enemLvlName.enabled = false;
            }
        }
	}
}
