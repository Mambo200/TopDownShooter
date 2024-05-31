using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_Speed;
    public float m_Damage;
    public float m_Lifetime;
    [HideInInspector]
    public int m_AdditionalBulletCollisions = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);


        m_Lifetime -= Time.deltaTime;
        if (m_Lifetime <= 0)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (enemy == null) return;

        enemy.TakeDamage(m_Damage);
        if (--m_AdditionalBulletCollisions < 0)
            Destroy(this.gameObject);

    }
}
