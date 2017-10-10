using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChage : MonoBehaviour {

    public GameObject firstSlot;
    public GameObject secondSlot;

    public bool wepInHands = false;

    private int currWep = 2;

    void Awake()
    {
        if (wepInHands)
        {
            if (currWep == 1)
            {
                firstSlot.SetActive(false);
            }
            else
                secondSlot.SetActive(false);
        }
    }


    void Update()
    {
        PlayerShooting cur = GetComponentInParent<PlayerShooting>();
        PlayerInventory inv = GetComponentInParent<PlayerInventory>();



        if (!cur.IsRealoading())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && inv.WeaponInSlot(true))
            {
                ChangeSlot(true, cur, inv);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && inv.WeaponInSlot(false))
            {
                ChangeSlot(false, cur, inv);
            }
        }
    }


    public void ChangeSlot(bool first , PlayerShooting cur, PlayerInventory inv)
    {
        if (first)
        {
            secondSlot.SetActive(false);
            firstSlot.SetActive(true);
            wepInHands = true;
            cur.WeaponSwitched(firstSlot.GetComponentInChildren<WeaponProp>());
            currWep = 0;
        }
        else
        {
            firstSlot.SetActive(false);
            secondSlot.SetActive(true);
            wepInHands = true;
            cur.WeaponSwitched(secondSlot.GetComponentInChildren<WeaponProp>());
            currWep = 1;
        }
    }


}
