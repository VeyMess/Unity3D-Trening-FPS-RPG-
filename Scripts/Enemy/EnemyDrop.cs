using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour {

    public float medChance = 50f;
    public float ammoChance = 60f;
    public float weaponChance = 40f;

    public GameObject medKit;
    public GameObject ammoBox;
    public GameObject AK47;
    public GameObject M4A1;

    private GameObject medDrop;
    private GameObject ammoDrop;
    private GameObject weaponDrop;

    public void SpawnDrop()
    {
        if (Random.Range(0f, 100f) <= medChance)
        {
            //spawn medkit
            Vector3 tempVec = transform.position;
            tempVec.y += 1.5f;
            medDrop = Instantiate(medKit, tempVec, Quaternion.LookRotation(Vector3.forward));
            medDrop.GetComponent<Rigidbody>().AddForce(new Vector3(0, 19, 7), ForceMode.Impulse);
            medDrop.layer = LayerMask.NameToLayer("PickUps");
        }
        if(Random.Range(0f,100f) <= ammoChance)
        {
            //spawn ammo
            Vector3 tempVec = transform.position;
            tempVec.y += 1.5f;
            ammoDrop = Instantiate(ammoBox, tempVec, Quaternion.LookRotation(Vector3.forward));
            ammoDrop.GetComponent<Rigidbody>().AddForce(new Vector3(0, 19, -7), ForceMode.Impulse);
            ammoDrop.layer = LayerMask.NameToLayer("PickUps");
        }
        if(Random.Range(0f,100f) <= weaponChance)
        {
            if (Random.Range(0f, 100f) < 40)
            {
                Vector3 tempVec = transform.position;
                tempVec.y += 1.5f;
                weaponDrop = Instantiate(AK47, tempVec, Quaternion.LookRotation(Vector3.forward));
                weaponDrop.AddComponent<PickUp>().itemType = PickUp.pickIttemType.Weapon_AK;
            }
            else
            {
                Vector3 tempVec = transform.position;
                tempVec.y += 1.5f;
                weaponDrop = Instantiate(M4A1, tempVec, Quaternion.LookRotation(Vector3.forward));
                weaponDrop.AddComponent<PickUp>().itemType = PickUp.pickIttemType.Weapon_M4;
            }
            weaponDrop.AddComponent<Rigidbody>();
            weaponDrop.GetComponent<Rigidbody>().mass = 3;
            weaponDrop.AddComponent<BoxCollider>();
            weaponDrop.GetComponent<Collider>().isTrigger = true;
            weaponDrop.GetComponent<Rigidbody>().AddForce(new Vector3(5, 19, -7), ForceMode.Impulse);
            weaponDrop.layer = LayerMask.NameToLayer("PickUps");
        }
    }
}
