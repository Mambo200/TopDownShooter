using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ShootProjectile))]
public class PlayerController : MonoBehaviour
{
    #region private
    CharacterController p_CharacterController;
    ShootProjectile p_ShootProjectile;
    [SerializeField]
    GameObject p_ShootStartPosition;
    [SerializeField]
    float p_ShootCooldown;
    [Header("Invincible Variables")]
    [SerializeField]
    float p_InvincibleTime;
    [SerializeField]
    float p_FlashTime;
    float p_CurrentFlashCooldown;
    [SerializeField]
    float p_AddMovementspeedWhileInvincivble;
    [SerializeField]
    Collider[] p_EnemyColliders;
    MeshRenderer[] p_AllPlayerCollidersMesh;
    #endregion
    #region public
    [Header("Generic")]
    public int m_Speed;
    public int m_RotationSpeed;
    [HideInInspector]
    public float m_CurrentShootCooldown;
    float m_CurrentInvincibleCooldown;
    Collider p_PlayerCollider;
    #endregion
    #region Property
    public Upgrades PlayerUpgrades { get; private set; }
    public bool IsInvincible { get => m_CurrentInvincibleCooldown > 0; }
    public float ShootCooldown { get => p_ShootCooldown; }
    #endregion


    #region Upgrades
    public float SpeedWithUpgrades { get => m_Speed * PlayerUpgrades.MoveSpeedMultiplierTotal; }
    public float ShootCooldownWithUpgrades { get => p_ShootCooldown * PlayerUpgrades.ShootSpeedMultiplierTotal; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        p_CharacterController = GetComponent<CharacterController>();
        p_ShootProjectile = GetComponent<ShootProjectile>();
        p_PlayerCollider = GetComponent<Collider>();

        p_AllPlayerCollidersMesh = GetComponentsInChildren<MeshRenderer>();

        PlayerUpgrades = new Upgrades();
    }

    // Update is called once per frame
    void Update()
    {
        if (UpgradesUIManager.Get.UpgradeWindowOpen
            || PauseUI.Get.Paused)
            return;
        Movement();
        Rotation();
        Shoot();
        InvincibleCheck();
    }

    private void Movement()
    {
        // get input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Player movement
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
        movement = movement.normalized;

        if (IsInvincible)
            movement *= p_AddMovementspeedWhileInvincivble;

        p_CharacterController.Move(movement * Time.deltaTime * SpeedWithUpgrades);
    }
    private void Rotation()
    {
        // Player rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle + 90, 0));
    }
    private void Shoot()
    {
        // reduce shoot cooldown
        m_CurrentShootCooldown = Mathf.Max(0, m_CurrentShootCooldown - Time.deltaTime);
        if (m_CurrentShootCooldown > 0) return;

        // could shoot. Check for key
        float fire = Input.GetAxisRaw("Fire1");
        // did not shoot
        if (fire == 0) return;

        // Shoot!
        p_ShootProjectile.Shoot(this.gameObject, p_ShootStartPosition.transform.position);
        //Debug.DrawRay(p_ShootStartPosition.transform.position, this.transform.forward * 10, Color.black, 1);
        m_CurrentShootCooldown = ShootCooldownWithUpgrades;
    }

    #region Invincible
    private void InvincibleCheck()
    {
        if (!IsInvincible)
            return;

        p_CurrentFlashCooldown -= Time.deltaTime;
        if (p_CurrentFlashCooldown <= 0)
            FlashTimer();

        m_CurrentInvincibleCooldown -= Time.deltaTime;
        if (m_CurrentInvincibleCooldown < 0)
            GetDefeatable();
    }

    private void GetInvincible()
    {
        m_CurrentInvincibleCooldown = p_InvincibleTime;
        p_CurrentFlashCooldown = p_FlashTime;
        for (int i = 0; i < p_EnemyColliders.Length; i++)
        {
            Physics.IgnoreCollision(p_PlayerCollider, p_EnemyColliders[i], true);
        }
    }
    private void GetDefeatable()
    {
        m_CurrentInvincibleCooldown = 0;
        for (int i = 0; i < p_EnemyColliders.Length; i++)
        {
            Physics.IgnoreCollision(p_PlayerCollider, p_EnemyColliders[i], false);
        }
        MakePlayerVisible();
    }
    #endregion

    #region Flash
    private void FlashTimer()
    {
        p_CurrentFlashCooldown = p_FlashTime;
        TogglePlayerVisibility();
    }
    private void MakePlayerInvisible()
    {
        for (int i = 0; i < p_AllPlayerCollidersMesh.Length; i++)
        {
            p_AllPlayerCollidersMesh[i].enabled = false;
        }
    }
    private void MakePlayerVisible()
    {
        for (int i = 0; i < p_AllPlayerCollidersMesh.Length; i++)
        {
            p_AllPlayerCollidersMesh[i].enabled = true;
        }
    }
    private void TogglePlayerVisibility()
    {
        for (int i = 0; i < p_AllPlayerCollidersMesh.Length; i++)
        {
            p_AllPlayerCollidersMesh[i].enabled = !p_AllPlayerCollidersMesh[i].enabled;
        }
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (enemy == null) return;

        if (IsInvincible)
            return;

        GetInvincible();
        MakePlayerInvisible();
    }
}
