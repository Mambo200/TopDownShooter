using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject m_ObjectToShoot;
    // Start is called before the first frame update
    void Start()
    {
        if (m_ObjectToShoot == null)
            Debug.LogWarning("No projectile", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject Shoot(GameObject _parent, Vector3 _start)
    {
        // create Object
        GameObject go = GameObject.Instantiate(m_ObjectToShoot, _start, _parent.transform.localRotation);
        Projectile p = go.GetComponent<Projectile>();
        if (p != null)
        {
            p.m_Damage *= EnemySpawner.Get.m_playerController.PlayerUpgrades.DamageMultiplierTotal;
            p.m_AdditionalBulletCollisions += EnemySpawner.Get.m_playerController.PlayerUpgrades.BulletCollisionTotal;
        }
        return go;
    }
}
