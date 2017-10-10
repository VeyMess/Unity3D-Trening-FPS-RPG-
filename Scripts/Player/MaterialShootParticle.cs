using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialShootParticle : MonoBehaviour {

    public GameObject hitBlood;
    public GameObject hitTerra;
    public GameObject hitStone;
    public GameObject hitWood;

    private Transform playerModel;
    private Terrain terTest;

    void Start()
    {
        playerModel = GetComponentInParent<PlayerMovement>().transform;
    }

    public void PlayerHit(RaycastHit rayHits, WeaponProp wepon)
    {
        foreach (EnemyMove enTemp in FindObjectsOfType<EnemyMove>())
        {
            if (Vector3.Distance(rayHits.point, enTemp.transform.position) <= 10f)
                enTemp.SetAgro(playerModel);
        }

        if (rayHits.collider.material.name.Equals("Meat (Instance)"))
        {
            EnemyHealth temp = rayHits.transform.GetComponent<EnemyHealth>();
            if (temp.ReturnHealth() > 0f)
            {
                GameObject part = Instantiate(hitBlood, rayHits.point, Quaternion.LookRotation(rayHits.normal));
                Destroy(part, 1.5f);
            }
            temp.TakeDamage(wepon.GetWeaponDMG(), playerModel);
        }
        else if (rayHits.collider.material.name.Equals("Stone (Instance)"))
        {
            GameObject part = Instantiate(hitStone, rayHits.point, Quaternion.LookRotation(rayHits.normal));
            Destroy(part, 3f);
        }
        else if (rayHits.transform.tag.Equals("Terra"))
        {
            Vector3 tempShoot = rayHits.point;
            terTest = rayHits.collider.GetComponent<Terrain>();
            tempShoot -= terTest.GetPosition();
            float pointHeight = terTest.terrainData.GetHeight(System.Convert.ToInt32(tempShoot.x), System.Convert.ToInt32(tempShoot.z));
            if (pointHeight + 0.5f < tempShoot.y)
            {
                GameObject tempPart = Instantiate(hitWood, rayHits.point, Quaternion.LookRotation(rayHits.normal));
                Destroy(tempPart, 3f);
                return;
            }
            GameObject part = Instantiate(hitTerra, rayHits.point, Quaternion.LookRotation(rayHits.normal));
            Destroy(part, 1.5f);
        }
        else
            HitEnemy(rayHits, wepon);
    }

    public void SetTerrain(Terrain ter)
    {
        terTest = ter;
    }

    public void HitEnemy(RaycastHit rayHits, WeaponProp wepon)
    {
        if(rayHits.collider.tag.Equals("Enemy"))
        {
            EnemyHitted tempHit = rayHits.collider.GetComponent<EnemyHitted>();
            if(tempHit.partType == EnemyHitted.PartOfEnemy.Chest)
            {
                EnemyHealth temp = rayHits.transform.GetComponentInParent<EnemyHealth>();
                if (temp.ReturnHealth() > 0f)
                {
                    GameObject part = Instantiate(hitTerra, rayHits.point, Quaternion.LookRotation(rayHits.normal));
                    Destroy(part, 1.5f);
                }
                temp.TakeDamage(wepon.GetWeaponDMG() * GetComponentInParent<PlayerInventory>().GetDMGRatio(), playerModel);
            }

            else if (tempHit.partType == EnemyHitted.PartOfEnemy.Head)
            {
                EnemyHealth temp = rayHits.transform.GetComponentInParent<EnemyHealth>();
                if (temp.ReturnHealth() > 0f)
                {
                    GameObject part = Instantiate(hitTerra, rayHits.point, Quaternion.LookRotation(rayHits.normal));
                    Destroy(part, 1.5f);
                }
                temp.TakeDamage((wepon.GetWeaponDMG() * GetComponentInParent<PlayerInventory>().GetDMGRatio()) * 1.5f, playerModel);
            }

            else if (tempHit.partType == EnemyHitted.PartOfEnemy.Limb)
            {
                EnemyHealth temp = rayHits.transform.GetComponentInParent<EnemyHealth>();
                if (temp.ReturnHealth() > 0f)
                {
                    GameObject part = Instantiate(hitTerra, rayHits.point, Quaternion.LookRotation(rayHits.normal));
                    Destroy(part, 1.5f);
                }
                temp.TakeDamage((wepon.GetWeaponDMG() * GetComponentInParent<PlayerInventory>().GetDMGRatio()) * .5f, playerModel);
            }
        }
    }
}
