using System.Collections;
using System.Collections.Generic;
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
    public Upgrades PlayerUpgrades { get; private set; }
    #endregion
    #region public
    public int m_Speed;
    public int m_RotationSpeed;
    [HideInInspector]
    public float m_CurrentShootCooldown;
    #endregion

    public float ShootCooldown { get => p_ShootCooldown; }

    #region Upgrades
    public float SpeedWithUpgrades { get => m_Speed * PlayerUpgrades.MoveSpeedMultiplierTotal; }
    public float ShootCooldownWithUpgrades { get => p_ShootCooldown * PlayerUpgrades.ShootSpeedMultiplierTotal; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        p_CharacterController = GetComponent<CharacterController>();
        p_ShootProjectile = GetComponent<ShootProjectile>();
        PlayerUpgrades = new Upgrades();

        //// TESTING: Adding upgrades
        //PlayerUpgrades.AddUpgradeWithEvent(new Upgrade(
        //    "Tap Dancer",
        //    _baseMovementSpeedMultiplier: 3,
        //    _baseShootIntervallMultiplier: 1.1f
        //    ));

        //PlayerUpgrades.AddUpgradeWithEvent(new Upgrade(
        //    "Machine Gun",
        //    _baseShootIntervallMultiplier: .75f
        //    ));

        //PlayerUpgrades.AddUpgradeWithEvent(new Upgrade(
        //    "Colossus",
        //    _baseMovementSpeedMultiplier: .5f,
        //    _baseShootIntervallMultiplier: 1.2f,
        //    _baseDamageMultiplier: 2
        //    ));

        //PlayerUpgrades.AddUpgradeWithEvent(new Upgrade(
        //    "Shotgun",
        //    _baseBulletCollideCount: 1
        //    ));

        Debug.Log($"Upgrades: {PlayerUpgrades.Enchantment.Count}");
        Debug.Log($"Movement: {PlayerUpgrades.MoveSpeedMultiplierTotal}");
        Debug.Log($"Shot speed: {PlayerUpgrades.ShootSpeedMultiplierTotal}");
        Debug.Log($"Damage: {PlayerUpgrades.DamageMultiplierTotal}");
        Debug.Log($"Bullet Collision: {PlayerUpgrades.BulletCollisionTotal}");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotation();
        Shoot();
    }

    private void Movement()
    {
        // get input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Player movement
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
        movement = movement.normalized;
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
}
