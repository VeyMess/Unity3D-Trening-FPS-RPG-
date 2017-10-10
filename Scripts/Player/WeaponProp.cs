using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProp : MonoBehaviour {

    public enum WeaponType { M4A1, AK47 };

    private float weaponDMG;
    private short ammoCappacity;
    private float shotDelay;
    private bool HasSight;

    public ParticleSystem flash;
    public AudioSource emptyMag;
    public AudioSource audShoot;
    public AudioSource audReloading;
    public WeaponType type;
    public int weaponLvL;

    private bool isShooting = false;
    private short currAmmo;

    

    void Awake()
    {
        int maxlvl = 0;
        foreach(PlayerInventory inv in FindObjectsOfType<PlayerInventory>())
        {
            if (inv.GetPlayerLvl() > maxlvl)
                maxlvl = inv.GetPlayerLvl();
        }

        weaponLvL = maxlvl;
        if(type == WeaponType.M4A1)
        {
            weaponDMG = 5f * Mathf.Pow(1.5f,weaponLvL);
            ammoCappacity = 30;
            shotDelay = .08f;
            HasSight = true;
        }
        if(type == WeaponType.AK47)
        {
            weaponDMG = 15f * Mathf.Pow(1.5f, weaponLvL);
            ammoCappacity = 30;
            shotDelay = .12f;
            HasSight = false;
        }
        currAmmo = ammoCappacity;
    }

    public void PlayParticleFlash()
    {
        flash.Play();
    }

    public float CheckDelay(float timeElapsed)
    {
        if (isShooting)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= shotDelay)
                isShooting = false;
        }
        return timeElapsed;
    }

    public void SetIsShooting(bool shoot)
    {
        isShooting = shoot;
    }

    public bool GetIsShooting()
    {
        return isShooting;
    }

    public string TotalPlusMag()
    {
            return currAmmo + "\\" + GetComponentInParent<PlayerInventory>().GetTottalAmmo(type);
    }

    public void Reloading()
    {
        if (GetComponentInParent<PlayerInventory>().GetTottalAmmo(type) - ammoCappacity > 0)
        {
            short need = (short)(ammoCappacity - currAmmo);
            currAmmo = ammoCappacity;
            GetComponentInParent<PlayerInventory>().ChageTotalAmmo(type, (short) -need);
        }
        else
        {
            currAmmo += GetComponentInParent<PlayerInventory>().GetTottalAmmo(type);
            GetComponentInParent<PlayerInventory>().ChageTotalAmmo(type,(short) -currAmmo);
        }
    }

    public bool MagIsEmpty()
    {
        return (currAmmo <= 0);
    }

    public void WeaponShoot()
    {
        --currAmmo;
    }

    public void AmmoPick(short amount)
    {
        GetComponentInParent<PlayerInventory>().AmmoPicked(amount, type);
    }

    public bool HasAmmo()
    {
        return (GetComponentInParent<PlayerInventory>().GetTottalAmmo(type) > 0);
    }

    public bool ClipIsFool()
    {
        return (currAmmo == ammoCappacity);
    }

    public float GetWeaponDMG()
    {
        return weaponDMG;
    }

    public void SetDamage(float dmg)
    {
        weaponDMG = dmg;
    }
}
