using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{   
    private AudioSource emptyMag;
    private AudioSource audShoot;
    private AudioSource audReloading;
    public Text magText;

    private WeaponProp wepInHand;
    private Camera playerCam;
    private float timeElapsed = 0f;
    private bool isReloading = false;

    void Start()
    {
        playerCam = GetComponent<Camera>();
    }

    void Update()
    {
        if (wepInHand != null && !GetComponentInParent<UIControlling>().GetInInventory())
        {
            if (wepInHand.GetIsShooting())
            {
                timeElapsed = wepInHand.CheckDelay(timeElapsed);
            }
            else if (Input.GetButton("Fire1") && !isReloading)
            {
                wepInHand.SetIsShooting(true);
                timeElapsed = 0f;
                PlayerShot();
            }

            if (Input.GetKeyDown(KeyCode.R) && !isReloading && !wepInHand.ClipIsFool())
            {
                if (wepInHand.HasAmmo())
                {
                    isReloading = true;
                    audReloading.Play();
                }
            }
            else if (isReloading && !audReloading.isPlaying)
            {
                wepInHand.Reloading();
                isReloading = false;
                magText.text = wepInHand.TotalPlusMag();
            }
        }
    }

    void PlayerShot()
    {
        RaycastHit rayHit;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out rayHit, Mathf.Infinity, layerMask) && !wepInHand.MagIsEmpty())
        {
            wepInHand.WeaponShoot();
            magText.text = wepInHand.TotalPlusMag();
            audShoot.Stop();
            audShoot.Play();
            wepInHand.PlayParticleFlash();
            wepInHand.GetComponent<MaterialShootParticle>().PlayerHit(rayHit, wepInHand);
        }
        else if (wepInHand.MagIsEmpty()) 
        {
            emptyMag.Play();
        }
        else
        {
            wepInHand.WeaponShoot();
            magText.text = wepInHand.TotalPlusMag();
            audShoot.Stop();
            audShoot.Play();
            wepInHand.PlayParticleFlash();
        }
    }

    public void AddAmmo(short amount)
    {
        // wepInHand.AmmoPick(amount);
        GetComponentInParent<PlayerInventory>().AmmoPicked(amount, wepInHand.type);
        magText.text = wepInHand.TotalPlusMag();
    }


    public void WeaponSwitched(WeaponProp wep)
    {
        wepInHand = wep;
        emptyMag = wepInHand.emptyMag;
        audShoot = wepInHand.audShoot;
        audReloading = wepInHand.audReloading;
        magText.text = wepInHand.TotalPlusMag();
    }


    public bool IsRealoading()
    {
        return isReloading;
    }

    public WeaponProp ReturnCurWeapon()
    {
        return wepInHand;
    }
}


