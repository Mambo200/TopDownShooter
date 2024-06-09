using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesUIManager : MonoBehaviour
{
    #region Singleton
    private static UpgradesUIManager p_Instance;
    public static UpgradesUIManager Get { get => p_Instance; }
    #endregion

    [SerializeField]
    private GameObject p_MainGameObject;
    [SerializeField]
    private GameObject p_ButtonParent;
    [SerializeField]
    private Button[] p_Buttons;
    private TMPro.TextMeshProUGUI[] p_ButtonsText;
    private void Awake()
    {
        if (p_Instance != null)
        {
            Debug.Log("There is already a " + nameof(UpgradesUIManager) + ". New instance will be deleted");
            Destroy(p_Instance.gameObject);
            return;
        }
        p_Instance = this;
        if (p_MainGameObject == null)
        {
            Debug.LogWarning("Main Gameobject ist not set", this);
            return;
        }
        if (p_ButtonParent == null)
        {
            Debug.LogWarning("Button parent ist not set", this);
            return;
        }
        if (p_Buttons == null || p_Buttons.Length == 0)
        {
            Debug.LogWarning("Buttons ist not set or has no content", this);
            return;
        }

        p_ButtonsText = new TextMeshProUGUI[p_Buttons.Length];
        for (int i = 0; i < p_Buttons.Length; i++)
        {
            p_ButtonsText[i] = p_Buttons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (p_ButtonsText[i] == null)
                Debug.LogWarning("No TextMeshPro found", this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DeactivateAllUIElements();
    }

    // Update is called once per frame
    void Update()
    {

    }




    Upgrade[] upgradesShown;
    /// <summary>
    /// Start showing upgrades
    /// </summary>
    /// <param name="_upgradecount">amount of upgrades shown. Max count is length of <see cref="p_Buttons"/></param>
    public void StartUpgrade(int _upgradecount)
    {
        Time.timeScale = 0;
        p_MainGameObject.SetActive(true);
        p_ButtonParent.SetActive(true);
        for (int i = 0; i < _upgradecount; i++)
        {
            p_Buttons[i].gameObject.SetActive(true);
        }
        upgradesShown = new Upgrade[_upgradecount];
        for (int i = 0; i < _upgradecount; i++)
        {
            upgradesShown[i] = GetRandomUpgrade();
        }

        // Set UI Text
        for (int i = 0; i < upgradesShown.Length; i++)
        {
            string toShow = upgradesShown[i].Name
                + Environment.NewLine
                + upgradesShown[i].Rarity.ToString();
            toShow += Environment.NewLine;

            if (upgradesShown[i].BaseMovementSpeedMultiplierActive)
                toShow += Environment.NewLine 
                    + "Add Movementspeed: " 
                    + upgradesShown[i].BaseMovementSpeedMultiplier;
            
            if (upgradesShown[i].BaseShootIntervallMultiplierActive)
                toShow += Environment.NewLine 
                    + "Add Shot speed: "
                    + upgradesShown[i].BaseShootIntervallMultiplier;
            
            if (upgradesShown[i].BaseDamageMultiplierActive)
                toShow += Environment.NewLine 
                    + "Add Damage: " 
                    + upgradesShown[i].BaseDamageMultiplier;
            
            if (upgradesShown[i].BaseBulletCollideCountActive)
                toShow = Environment.NewLine 
                    + "Add projectile collide count: " 
                    + upgradesShown[i].BaseBulletCollideCount;
        }
    }
    public void StopUpgrade()
    {
        Time.timeScale = 1;
        DeactivateAllUIElements();
    }
    private void DeactivateAllUIElements()
    {
        for (int i = 0; i < p_ButtonsText.Length; i++)
        {
            p_Buttons[i].gameObject.SetActive(false);
        }
        p_ButtonParent.SetActive(false);
        p_MainGameObject.SetActive(false);
    }

    private Upgrade GetRandomUpgrade()
    {
        int upgradeCount = 0;
        Upgrade upgrade = new Upgrade();

        int range = 0;

        const int movespeedMaxRange = 15;
        const int shotspeedMaxRange = 10;
        const int damageMaxRange = 10;
        const int bulletMaxRange = 100;
        const int onebulletMaxRange = 95;
        const int twobulletsMaxRange = 100;

        // Move speed
        if (UnityEngine.Random.Range(0, upgradeCount + 1) == 0)
        {
            range = UnityEngine.Random.Range(1, movespeedMaxRange);
            upgrade.BaseMovementSpeedMultiplier = (float)range / 100 + 1;
            upgrade.BaseMovementSpeedMultiplierActive = true;
            upgradeCount++;
        }
        // Shot speed
        if (UnityEngine.Random.Range(0, upgradeCount + 1) == 0)
        {
            range = UnityEngine.Random.Range(1, shotspeedMaxRange);
            upgrade.BaseShootIntervallMultiplier = (float)range / 100 + 1;
            upgrade.BaseShootIntervallMultiplierActive = true;
            upgradeCount++;
        }

        // Damage
        if (UnityEngine.Random.Range(0, upgradeCount + 1) == 0)
        {
            range = UnityEngine.Random.Range(1, damageMaxRange);
            upgrade.BaseDamageMultiplier = (float)range / 100 + 1;
            upgrade.BaseDamageMultiplierActive = true;
        }

        // Bullet count
        range = UnityEngine.Random.Range(0, bulletMaxRange);
        if (range >= onebulletMaxRange)
        {
            upgrade.BaseBulletCollideCount++;
            upgrade.BaseBulletCollideCountActive = true;
            upgradeCount++;
        }
        if (range >= twobulletsMaxRange)
        {
            upgrade.BaseBulletCollideCount++;
            upgrade.BaseBulletCollideCountActive = true;
            upgradeCount++;
        }

        upgrade.Rarity = (Rarity)upgradeCount;
        upgrade.Name = "Upgrade";

        if (upgradeCount == 0)
        {
            // no upgrades generated --> get maximum Upgrade (but still only ont bullet collide count)
            upgrade.Name = "Jackpot";
            upgrade.BaseMovementSpeedMultiplier = (float)movespeedMaxRange / 100 + 1;
            upgrade.BaseMovementSpeedMultiplierActive = true;
            upgradeCount++;
            upgrade.BaseShootIntervallMultiplier = (float)shotspeedMaxRange / 100 + 1;
            upgrade.BaseShootIntervallMultiplierActive = true;
            upgradeCount++;
            upgrade.BaseDamageMultiplier = (float)damageMaxRange / 100 + 1;
            upgrade.BaseDamageMultiplierActive = true;
            upgradeCount++;
            upgrade.BaseBulletCollideCount++;
            upgrade.BaseBulletCollideCountActive = true;
            upgradeCount++;

            upgrade.Rarity = Rarity.LEGENDARY;
        }

        return upgrade;
    }

    /// <summary>
    /// Add Keyword in Text
    /// </summary>
    /// <param name="_keyword">Keyword to add</param>
    /// <param name="_text">Text</param>
    /// <remarks>https://youtu.be/N6vYyCahLr8</remarks>
    /// <returns>Text with keyword</returns>
    public static string CreateKeywordWithText(string _keyword, string _text)
    {
        return $"<link=\"{_keyword}\">{_text}</link>";
    }
    /// <summary>
    /// Set text to textelement
    /// </summary>
    /// <param name="_buttonNumber">Put text in textelement in <see cref="p_ButtonsText"/></param>
    /// <param name="_text">Text to show</param>
    public void SetText(int _buttonNumber, string _text)
    {
        p_ButtonsText[_buttonNumber].SetText(_text);
    }
    public void AquireUpgrade(int _buttonIndex)
    {
        EnemySpawner.Get.m_playerController.PlayerUpgrades.AddUpgradeWithEvent(upgradesShown[_buttonIndex]);
    }
}
