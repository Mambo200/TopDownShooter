using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ShootProjectile))]
public class PlayerController : MonoBehaviour, IContact
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
    [Header("Health")]
    [SerializeField]
    private float m_Health;
    [SerializeField]
    private float m_Armor;
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
    public float Health { get => m_Health; }
    public float Armor { get => m_Armor; }
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

    #region Receiving damage or heal
    /// <summary>
    /// Player receives damage
    /// </summary>
    /// <param name="_rawDamage">damage player should receive</param>
    public void ReceiveDamage(float _rawDamage)
    {
        float damageInput = DamageCalculation(_rawDamage, m_Armor);
        damageInput = BeforeReceivingDamage(_rawDamage , damageInput);
        m_Health -= damageInput;
        m_Health = AfterReceivingDamage(m_Health);

        // death check
        if (m_Health <= 0)
            OnDeath();
    }

    public void Heal(float _healamount)
    {
        m_Health += _healamount;
    }
    private void OnDeath()
    {
        
    }
    /// <summary>
    /// This will be called right before player will get damaged
    /// </summary>
    /// <param name="_rawDamage">Raw damage</param>
    /// <param name="_damageReceive">Damage receive after calculation. This is the damage the player receives</param>
    /// <returns>the damage the player will get. If no specialty occurs this should be "_damageReceive"</returns>
    private float BeforeReceivingDamage(float _rawDamage, float _damageReceive)
    {
        return _damageReceive;
    }
    /// <summary>
    /// This will be called right before player will get damaged. Death-messages are not applyed by now.
    /// </summary>
    /// <param name="_newHealth">new health the player has</param>
    /// <returns>new health of player</returns>
    private float AfterReceivingDamage(float _newHealth)
    {
        return _newHealth;
    }
    /// <summary>
    /// Calculates the damage
    /// </summary>
    /// <param name="_damage">Damage receive</param>
    /// <param name="_armor">Armor of player</param>
    /// <returns>New damage</returns>
    private static float DamageCalculation(float _damage, float _armor)
    {
        return Mathf.Max(_damage - _armor, 0);
    }
    #endregion
}
