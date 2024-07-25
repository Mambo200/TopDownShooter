using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("GO Spawn")]
    [SerializeField]
    GameObject[] p_ToSpawn;
    [SerializeField]
    GameObject[] p_MiniBossToSpawn;

    public GameObject m_playerGameObject { get; private set; }
    public PlayerController m_playerController { get; private set; }
    
    [Header("Spawn Variables")]
    [SerializeField]
    float p_SpawnRadius;
    [SerializeField]
    float p_MaxEnemySpawnInterval;
    public int m_EnemyKillCountToUpgradeModulo = 10;

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

            GameObject spawned = SpawnEnemy(GetRandomEnemy());
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
        KillCountUIManager.Get.SetKillCountText(p_Killcount.ToString());
        if (p_Killcount % m_EnemyKillCountToUpgradeModulo == 0)
            UpgradesUIManager.Get.StartUpgrade(3);
    }

    /// <summary>Get random enemy from <see cref="p_ToSpawn"/></summary>
    /// <returns>Prefab</returns>
    public GameObject GetRandomEnemy() => p_ToSpawn[Random.Range(0, p_ToSpawn.Length)];
    /// <summary>Get random enemy from <see cref="p_MiniBossToSpawn"/></summary>
    /// <returns>Prefab</returns>
    public GameObject GetRandomMiniBoss() => p_MiniBossToSpawn[Random.Range(0, p_MiniBossToSpawn.Length)];


    public GameObject SpawnEnemy(GameObject _toSpawn)
    {
        Vector3 spawnPosition;
        GameObject spawned = null;
        if (TryGetRandomNavMeshPosition(out spawnPosition))
        {
            spawned = Instantiate(_toSpawn, spawnPosition, Quaternion.identity, this.gameObject.transform);
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
