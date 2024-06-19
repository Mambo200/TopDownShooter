using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] p_ToSpawn;
    public GameObject m_playerGameObject { get; private set; }
    public PlayerController m_playerController { get; private set; }
    [SerializeField]
    float p_SpawnRadius;
    [SerializeField]
    float p_MaxEnemySpawnInterval;

    private static EnemySpawner m_Instance;
    public static EnemySpawner Get { get => m_Instance; }

    private int p_Killcount;

    public float m_MinSpawnDistanceFromPlayer;
    [HideInInspector]
    public float m_CurrentEnemySpawnInterval;


    private void Awake()
    {
        if(m_Instance != null)
        {
            Debug.LogWarning("There is already an enemy spawner!", this.gameObject);
            Debug.Break();
        }
        m_Instance = this;

        m_playerGameObject = GameObject.FindGameObjectWithTag("Player");
        m_playerController = m_playerGameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        m_CurrentEnemySpawnInterval -= Time.deltaTime;
        if(m_CurrentEnemySpawnInterval <= 0)
        {
            GameObject spawned = SpawnEnemy();
            m_CurrentEnemySpawnInterval = p_MaxEnemySpawnInterval;
            //if(spawned)
            //    Debug.Log("Enemy spawned", spawned);
            //else
            //    Debug.LogWarning("No Enemy spawned", spawned);
        }

    }

    public int GetKillCount() => p_Killcount;
    public void AddToKillCount()
    {
        p_Killcount++;
        Debug.Log(p_Killcount);
        if (p_Killcount % 5 == 0)
            UpgradesUIManager.Get.StartUpgrade(3);
    }


    public GameObject SpawnEnemy()
    {
        Vector3 spawnPosition;
        GameObject spawned = null;
        if (TryGetRandomNavMeshPosition(out spawnPosition))
        {
            int toSpawn = Random.Range(0, p_ToSpawn.Length);
            spawned = Instantiate(p_ToSpawn[toSpawn], spawnPosition, Quaternion.identity, this.gameObject.transform);
        }

        return spawned;
    }

    bool TryGetRandomNavMeshPosition(out Vector3 result)
    {
        for (int i = 0; i < 30; i++) // Max 30 Versuche
        {
            Vector3 randomPoint = m_playerGameObject.transform.position + Random.insideUnitSphere * p_SpawnRadius;
            randomPoint.y = 0;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                float distance = Vector3.Distance(hit.position, m_playerGameObject.transform.position);
                if (distance >= m_MinSpawnDistanceFromPlayer)
                {
                    result = hit.position;
                    return true;
                }
            }
        }

        result = Vector3.zero;
        return false;
    }

    public int CountEnemies()
    {
        return this.gameObject.GetComponentsInChildren<Transform>().Length;
    }
}
