using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private bool inAir = false;

    public enum pickIttemType { Health,Ammo,Weapon_AK,Weapon_M4};
    public pickIttemType itemType;

    void Start()
    {
        Destroy(gameObject, 60);
    }


    void FixedUpdate()
    {
        RaycastHit rayhit;

        if(Physics.Raycast(transform.position,-transform.up,out rayhit))
        {
            if(Vector3.Distance(rayhit.point,transform.position)<1f && rayhit.collider.tag.Equals("Terra") && !inAir)
            {
                Rigidbody temp = GetComponent<Rigidbody>();
                temp.Sleep();
                temp.useGravity = false;
                inAir = true;
            }
            if(inAir)
            {
                transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag.Equals("Player"))
        {
            if(itemType == pickIttemType.Health)
            {
                coll.GetComponent<PlayerHealth>().HealthChange(25);
                Destroy(gameObject);
            }
            if(itemType == pickIttemType.Ammo)
            {
                coll.GetComponentInChildren<PlayerShooting>().AddAmmo((short)30);
                Destroy(gameObject);
            }
            if(itemType == pickIttemType.Weapon_AK)
            {
                coll.GetComponent<PlayerInventory>().WeaponPickUp(pickIttemType.Weapon_AK);
                Destroy(gameObject);
            }
            if (itemType == pickIttemType.Weapon_M4)
            {
                coll.GetComponent<PlayerInventory>().WeaponPickUp(pickIttemType.Weapon_M4);
                Destroy(gameObject);
            }
        }
    }
}
