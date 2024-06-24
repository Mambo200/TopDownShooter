using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    #region Singleton
    private static PauseUI p_Instance;
    public static PauseUI Get { get => p_Instance; }
    #endregion

    [SerializeField]
    private GameObject p_MainPauseGameObject;
    [SerializeField]
    private TMPro.TextMeshProUGUI p_UpgradesText;

    [SerializeField]
    private GameObject p_ProjectilePrefab;
    private float p_ProjectileBaseDamage;


    public bool Paused { get; set; }

    float p_LastAxis = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        if (p_Instance != null)
        {
            Debug.LogWarning($"There is already an instance of {nameof(GameOverUIManager)}, it will be deleted.");
            Destroy(this.gameObject);
            return;
        }
        p_Instance = this;
    }
    void Start()
    {
        p_ProjectileBaseDamage = p_ProjectilePrefab.GetComponent<Projectile>().m_Damage;
        p_MainPauseGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float current = Input.GetAxisRaw("Cancel");
        if (current == 1
            && p_LastAxis == 0)
            PausePressed();

        p_LastAxis = current;
    }

    public void PausePressed()
    {
        if (UpgradesUIManager.Get.UpgradeWindowOpen
            || GameOverUIManager.Get.IsGameOver)
            return;

        if (Paused)
            GameResumed();
        else
            GamePaused();
    }

    private float tempTimeScale = 0;
    public void GamePaused()
    {
        Paused = true;
        p_MainPauseGameObject.SetActive(true);
        tempTimeScale = Time.timeScale;
        Time.timeScale = 0;
        SetUpgradesTextOnPauseUI();
    }
    public void GameResumed()
    {
        Paused = false;
        Time.timeScale = tempTimeScale;
        p_MainPauseGameObject.SetActive(false);
    }

    private void SetUpgradesTextOnPauseUI()
    {
        p_UpgradesText.text = GetUpgradesText();
    }
    private string GetUpgradesText()
    {
        string tr = "Movespeed: "
            + EnemySpawner.Get.m_playerController.PlayerUpgrades.MoveSpeedMultiplierTotal * EnemySpawner.Get.m_playerController.m_Speed
            + " ("
            + EnemySpawner.Get.m_playerController.m_Speed
            + " * "
            + EnemySpawner.Get.m_playerController.PlayerUpgrades.MoveSpeedMultiplierTotal
            + ")";
        tr += Environment.NewLine;

        tr += "Shoot speed: "
            + EnemySpawner.Get.m_playerController.PlayerUpgrades.ShootSpeedMultiplierTotal * EnemySpawner.Get.m_playerController.ShootCooldown
            + " sec"
            + " ("
            + EnemySpawner.Get.m_playerController.ShootCooldown
            + " * "
            + EnemySpawner.Get.m_playerController.PlayerUpgrades.ShootSpeedMultiplierTotal
            + ")";
        tr += Environment.NewLine;

        tr += "Damage: "
            + EnemySpawner.Get.m_playerController.PlayerUpgrades.DamageMultiplierTotal * p_ProjectileBaseDamage
            + " ("
            + p_ProjectileBaseDamage
            + " * "
            + EnemySpawner.Get.m_playerController.PlayerUpgrades.DamageMultiplierTotal
            + ")";
        tr += Environment.NewLine;

        tr += "Bullet collide count: "
            + EnemySpawner.Get.m_playerController.PlayerUpgrades.BulletCollisionTotal;

        return tr;
    }
}