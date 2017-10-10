using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControlling : MonoBehaviour
{
    private bool inInventory = false;

    public Canvas mainUI;
    public Canvas inventoryUI;
    public GameObject inventoryPanel;
    public GameObject onCharFirstSlot;
    public GameObject onCharSecondSlot;
    public GameObject charStatsSlot;
    public GameObject upHealthButton;
    public GameObject upRatioButton;
    private List<GameObject> inventoryStorage = new List<GameObject>();

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && !inInventory)
        {
            mainUI.gameObject.SetActive(false);
            inventoryUI.gameObject.SetActive(true);
            inInventory = true;
            ResetInventory();
            UpdatePlayerStats(charStatsSlot);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;         
        }
        else if(Input.GetKeyDown(KeyCode.Tab) && inInventory)
        {
            foreach(GameObject tempObj in inventoryStorage)
            {
                Destroy(tempObj);
            }
            mainUI.gameObject.SetActive(true);
            inventoryUI.gameObject.SetActive(false);
            inInventory = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }

    public void ChangeSlot(GameObject weapon)
    {
        GetComponent<PlayerInventory>().SetWeaponInSlot(true, weapon, onCharFirstSlot);
    }

    public void ChaNgeSlot2(GameObject weapon)
    {
        GetComponent<PlayerInventory>().SetWeaponInSlot(false, weapon, onCharSecondSlot);
    }

    public void ResetInventory()
    {
        if (inventoryStorage.Count > 0)
        {
            foreach (GameObject tempObj in inventoryStorage)
            {
                Destroy(tempObj);
            }
        }
        inventoryStorage = GetComponent<PlayerInventory>().SetInventoryMenu(inventoryPanel, gameObject);
    }

    public void UpdatePlayerStats(GameObject charStats)
    {
        PlayerHealth plHP = GetComponent<PlayerHealth>();
        PlayerInventory plStats = GetComponent<PlayerInventory>();

        foreach(Text textTemp in charStats.GetComponentsInChildren<Text>())
        {
            switch(textTemp.name)
            {
                case "Health":
                    textTemp.text = "Health: " + plHP.GetPlayerHealth();
                    break;
                case "MaxHealth":
                    textTemp.text = "MaxHP: " + plHP.GetPlayerMaxHP();
                    break;
                case "DmgRatio":
                    textTemp.text = "DMG: X" + plStats.GetDMGRatio();
                    break;
                case "CurXP":
                    textTemp.text = "XP: " + plStats.GetXP();
                    break;
                case "CurrLVL":
                    textTemp.text = "LVL: " + plStats.GetPlayerLvl();
                    break;
                case "NeedXP":
                    textTemp.text = "UP(XP): " + plStats.GetNextLvlXP();
                    break;
                default:
                    break;
            }
        }

        if(plStats.GetUnspentPoints() > 0)
        {
            upHealthButton.SetActive(true);
            upRatioButton.SetActive(true);
        }
        else
        {
            upHealthButton.SetActive(false);
            upRatioButton.SetActive(false);
        }
    }

    public void UpHealthButtton()
    {
        GetComponent<PlayerInventory>().SpendOnePoint();
        GetComponent<PlayerHealth>().MaxHealthChange(10f);
        UpdatePlayerStats(charStatsSlot);
    }

    public void UpRatioButton()
    {
        PlayerInventory tempInv = GetComponent<PlayerInventory>();
        tempInv.SpendOnePoint();
        tempInv.UpDMGRatio(.05f);

        UpdatePlayerStats(charStatsSlot);
    }

    public bool GetInInventory()
    {
        return inInventory;
    }
}
