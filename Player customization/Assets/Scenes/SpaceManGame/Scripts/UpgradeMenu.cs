using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private Text fireRateText;

    [SerializeField]
    private float healthMultiplier = 1.3f;

    [SerializeField]
    private float movementSpeedMultiplier = 1.1f;

    [SerializeField]
    private float fireRateMultiplier = 1.1f;

    [SerializeField]
    private int upgradeCost = 50;

    [SerializeField]
    private int fireRateCost = 75;

    private PlayerStats stats;


    private void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "HEALTH: " + stats.maxHealth.ToString();
        speedText.text = "SPEED: " + stats.movementSpeed.ToString();
        fireRateText.text = "FIRE RATE: " + stats.fireRate.ToString();
    }

    public void UpgradeHealth()
    {
        if (GameMaster.Money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        stats.maxHealth = (int)(healthMultiplier * stats.maxHealth);

        GameMaster.Money -= upgradeCost;
        AudioManager.instance.PlaySound("Money");

        UpdateValues();
        
    }

    public void UpgradeSpeed()
    {
        if (GameMaster.Money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        stats.movementSpeed = Mathf.Round(stats.movementSpeed * movementSpeedMultiplier);

        GameMaster.Money -= upgradeCost;
        AudioManager.instance.PlaySound("Money");

        UpdateValues();
    }

    public void UpgradeFireRate()
    {
        if (GameMaster.Money < fireRateCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        stats.fireRate = Mathf.Round(stats.fireRate * fireRateMultiplier);

        GameMaster.Money -= fireRateCost;
        if (stats.fireRate >= 7)
        {
            AudioManager.instance.PlaySound("GunLevelUp");
        }
        else
        {
            AudioManager.instance.PlaySound("Money");
        }
        

        UpdateValues();
    }
}
