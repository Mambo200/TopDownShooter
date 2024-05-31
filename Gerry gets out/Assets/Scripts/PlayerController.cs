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

    // Start is called before the first frame update
    void Start()
    {
        p_CharacterController = GetComponent<CharacterController>();
        p_ShootProjectile = GetComponent<ShootProjectile>();
        PlayerUpgrades = new Upgrades();
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
        p_CharacterController.Move(movement * Time.deltaTime * m_Speed * PlayerUpgrades.MoveSpeedMultiplierTotal);
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
        m_CurrentShootCooldown = p_ShootCooldown;
    }
}
