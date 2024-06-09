using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class BaseEnemy : MonoBehaviour
{
    public static PlayerController m_PlayerController { get; private set; }
    public static GameObject m_playerGameObject { get; private set; }

    [SerializeField]
    protected float m_MaxHealth;
    protected float m_CurrentHealth;
    [SerializeField]
    protected float m_Armor;

    protected void Awake()
    {
        if (m_PlayerController != null)
            return;

        m_playerGameObject = GameObject.FindGameObjectWithTag("Player");
        m_PlayerController = m_playerGameObject.GetComponent<PlayerController>();
    }
    
    protected NavMeshAgent m_NavMeshAgent;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        m_CurrentHealth = m_MaxHealth;
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        // move to player. If this is not wanted use "isStopped = true" on child
        m_NavMeshAgent.destination = m_playerGameObject.transform.position;
    }

    virtual public bool TakeDamage(float _damage)
    {
        if (_damage < 0) return false;

        m_CurrentHealth -= CalculateDamageWithArmor(_damage);
        if (m_CurrentHealth <= 0)
        {
            Death();
            return true;
        }
        else
            return false;
    }
    virtual public void Heal(float _healAmount)
    {
        m_CurrentHealth = Mathf.Min(m_MaxHealth, m_CurrentHealth + _healAmount);
    }

    virtual protected float CalculateDamageWithArmor(float _rawDamage)
    {
        return Mathf.Max(0, _rawDamage - m_Armor);
    }

    virtual protected void Death()
    {
        EnemySpawner.Get.AddToKillCount();
        Destroy(this.gameObject);
    }
}
