using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {

    private int playerLvl = 1;
    private int curEXP = 0;
    private int nextLvlExp = 100;
    private int statPoints = 0;
    private float dmgRatio = 1f;

    private short maxAmmoM4 = 400;
    private short totallAmmoM4 = 150;
    private short maxAmmoAK = 300;
    private short totallAmmoAK = 120;

    public Slider xpSlider;
    public Text levelText;

    List<WeaponItem> weapons = new List<WeaponItem>();

    WeaponProp firstSlot;
    WeaponProp secondSlot;

    public WeaponProp AK47FAB;
    public WeaponProp M4A1FAB;

    public GameObject CreateAK;
    public GameObject CreateM4;

    public GameObject uIItem;

    void Start()
    {
        SliderUpdate();
    }

    public void UpDMGRatio(float amount)
    {
        dmgRatio += amount;
    }

    public void SpendOnePoint()
    {
        --statPoints;
    }

    public int GetUnspentPoints()
    {
        return statPoints;
    }

    public float GetDMGRatio()
    {
        return dmgRatio;
    }

    public int GetXP()
    {
        return curEXP;
    }

    public int GetNextLvlXP()
    {
        return nextLvlExp;
    }

    public void GiveEXP(int amount)
    {
        curEXP += amount;
        if (curEXP >= nextLvlExp)
        {
            curEXP -= nextLvlExp;
            nextLvlExp *= 2;
            playerLvl++;
            statPoints++;

            SliderUpdate();
        }
        else
            xpSlider.value = curEXP;
    }

    void SliderUpdate()
    {
        xpSlider.maxValue = nextLvlExp;
        xpSlider.value = curEXP;
        levelText.text = playerLvl + " LvL";
    }

    public int GetPlayerLvl()
    {
        return playerLvl;
    }

    public void WeaponPickUp(PickUp.pickIttemType tempType)
    {
        WeaponProp tempWep;
        if (tempType == PickUp.pickIttemType.Weapon_AK)
        {
            tempWep = Instantiate(AK47FAB);
            weapons.Add(new WeaponItem(WeaponItem.Type.Ak47, tempWep.weaponLvL, tempWep.GetWeaponDMG()));
            Destroy(tempWep.gameObject);
        }
        else if (tempType == PickUp.pickIttemType.Weapon_M4)
        {
            tempWep = Instantiate(M4A1FAB);
            weapons.Add(new WeaponItem(WeaponItem.Type.M4A1, tempWep.weaponLvL, tempWep.GetWeaponDMG()));
            Destroy(tempWep.gameObject);
        }
    }

    public short GetTottalAmmo(WeaponProp.WeaponType temp)
    {
        if (temp == WeaponProp.WeaponType.AK47)
            return totallAmmoAK;
        else
            return totallAmmoM4;
    }

    public void ChageTotalAmmo(WeaponProp.WeaponType temp , short amount)
    {
        if (temp == WeaponProp.WeaponType.AK47)
            totallAmmoAK += amount;
        else
            totallAmmoM4 += amount;
    }

    public void AmmoPicked(short amount, WeaponProp.WeaponType type)
    {
        if (type == WeaponProp.WeaponType.AK47)
        {
            if ((totallAmmoAK + amount) < maxAmmoAK)
            {
                totallAmmoAK += amount;
            }
            else
                totallAmmoAK = maxAmmoAK;
        }
        else
        {
            if ((totallAmmoM4 + amount) < maxAmmoM4)
            {
                totallAmmoM4 += amount;
            }
            else
                totallAmmoM4 = maxAmmoAK;
        }
    }

    public bool WeaponInSlot(bool first)
    {
        if (secondSlot != null && !first)
            return true;
        else if (firstSlot != null && first)
            return true;
        else
            return false;
    }

    public WeaponProp ReturnWeaponInSlot (WeaponProp.WeaponType temp)
    {
        if(temp == WeaponProp.WeaponType.AK47)
        {
            return secondSlot;
        }
        else
        {
            return firstSlot;
        }
    }

    public void SetTotalAmmo(WeaponProp.WeaponType temp, short amount)
    {
        if(temp == WeaponProp.WeaponType.AK47)
            totallAmmoAK = amount;
        if(temp == WeaponProp.WeaponType.M4A1)
            totallAmmoM4 = amount;
    }

    public List<GameObject> SetInventoryMenu(GameObject inventPanel, GameObject uiContr)
    {
        List<GameObject> tempInv = new List<GameObject>();
        foreach(WeaponItem wepIt in weapons)
        {
            GameObject tempObj = Instantiate(uIItem,inventPanel.transform);
            foreach(Text tempTxt in tempObj.GetComponentsInChildren<Text>())
            {
                if (tempTxt.name.Equals("WeaponType"))
                    tempTxt.text = wepIt.weaponType.ToString();
                else if (tempTxt.name.Equals("DMG"))
                    tempTxt.text = (((float)((int)(wepIt.weaponDMG * 100))) / 100f).ToString();
                else if (tempTxt.name.Equals("LVL"))
                    tempTxt.text = wepIt.weaponLvl.ToString();
            }
            foreach(Button tBut in tempObj.GetComponentsInChildren<Button>())
            {
                if(tBut.name.Equals("Button1Slot"))
                {
                    tBut.onClick.AddListener(() => FindObjectOfType<UIControlling>().ChangeSlot(tempObj));
                }
                else if(tBut.name.Equals("Button2Slot"))
                {
                    tBut.onClick.AddListener(() => FindObjectOfType<UIControlling>().ChaNgeSlot2(tempObj));
                }
            }
            tempInv.Add(tempObj);
        }
        return tempInv;
    }

    public void SetWeaponInSlot(bool first, GameObject weap, GameObject charSlot)
    {
        GameObject wepSlot;
        WeaponItem tWeapon = new WeaponItem();

        int count = 0;
        foreach (Text temp in weap.GetComponentsInChildren<Text>())
        {
            if (temp.name.Equals("WeaponType"))
            {
                if (temp.text == "Ak47")
                    tWeapon.weaponType = WeaponItem.Type.Ak47;
                else
                    tWeapon.weaponType = WeaponItem.Type.M4A1;
                count++;
            }
            else if (temp.name.Equals("DMG"))
            {
                string tDmg = "";
                foreach (char c in temp.text)
                {
                    if (char.IsDigit(c) || c == '.')
                        tDmg += c;
                }
                tWeapon.weaponDMG = System.Convert.ToSingle(tDmg);
                count++;
            }
            else if (temp.name.Equals("LVL"))
            {
                string tLVL = "";
                foreach (char c in temp.text)
                    if(char.IsDigit(c))
                        tLVL += c;
                tWeapon.weaponLvl = System.Convert.ToInt32(tLVL);
                count++;
            }

            if (count == 3)
                break;
        }

        int counter =0;
        foreach (WeaponItem wepTe in weapons)
        {
            if(wepTe.weaponType == tWeapon.weaponType)
                if(wepTe.weaponLvl == tWeapon.weaponLvl)
                {
                    weapons.RemoveAt(counter);
                    break;
                }
            counter++;
        }

        if (first)
        {
            wepSlot = GetComponentInChildren<WeaponChage>().firstSlot;
            if (firstSlot != null)
            {
                FromCharToBag(charSlot);
                Destroy(wepSlot.GetComponentInChildren<WeaponProp>().gameObject);
            }
        }
        else
        {
            wepSlot = GetComponentInChildren<WeaponChage>().secondSlot;
            if (secondSlot != null)
            {
                FromCharToBag(charSlot);
                Destroy(wepSlot.GetComponentInChildren<WeaponProp>().gameObject);
            }
        }

        GameObject weaponinpos;
        if(tWeapon.weaponType == WeaponItem.Type.Ak47)
        {
            weaponinpos = Instantiate(CreateAK, wepSlot.transform);
        }
        else
        {
            weaponinpos = Instantiate(CreateM4, wepSlot.transform);
        }

        weaponinpos.GetComponent<WeaponMove>().playerAnim = GetComponent<Animator>();
        weaponinpos.GetComponent<WeaponMove>().mainCam = GetComponentInChildren<Camera>();

        WeaponProp tProp = weaponinpos.GetComponent<WeaponProp>();
        tProp.SetDamage(tWeapon.weaponDMG);
        tProp.weaponLvL = tWeapon.weaponLvl;

        if (first)
            firstSlot = tProp;
        else
            secondSlot = tProp;

        ShowInSlot(tWeapon, charSlot);
        GetComponent<UIControlling>().ResetInventory();
    }

    private void FromCharToBag(GameObject slotUI)
    {
        WeaponItem wepIt = new WeaponItem();
        foreach (Text temp in slotUI.GetComponentsInChildren<Text>())
        {
            if (temp.name.Equals("WeaponType"))
            {
                if (temp.text == "Ak47")
                    wepIt.weaponType = WeaponItem.Type.Ak47;
                else
                    wepIt.weaponType = WeaponItem.Type.M4A1;
            }
            else if (temp.name.Equals("WeaponDmg"))
            {
                string tempdmg = "";
                foreach (char c in temp.text)
                    if (char.IsDigit(c) || c == '.')
                        tempdmg += c;
                wepIt.weaponDMG = System.Convert.ToSingle(tempdmg);
            }
            else
            {
                string tempLvl = "";
                foreach (char c in temp.text)
                    if (char.IsDigit(c))
                        tempLvl += c;
                wepIt.weaponLvl = System.Convert.ToInt32(tempLvl);
            }
        }
        weapons.Add(wepIt);
    }

    private void ShowInSlot(WeaponItem wep, GameObject slotUI)
    {
        foreach(Text temp in slotUI.GetComponentsInChildren<Text>())
        {
            if(temp.name.Equals("WeaponType"))
            {
                temp.text = wep.weaponType.ToString();
            }
            else if(temp.name.Equals("WeaponDmg"))
            {
                temp.text = "DMG:" + wep.weaponDMG;
            }
            else
            {
                temp.text = "LVL:" + wep.weaponLvl;
            }
        }
    }
}

public class WeaponItem
{
    public enum Type { Ak47,M4A1};

    public Type weaponType;
    public int weaponLvl;
    public float weaponDMG;

    public WeaponItem()
    { }

    public WeaponItem(Type type, int lvl, float dmg)
    {
        weaponType = type;
        weaponLvl = lvl;
        weaponDMG = dmg;
    }
}
