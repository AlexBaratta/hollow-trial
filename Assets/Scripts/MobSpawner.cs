using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MobSpawnEntry
{
    public GameObject prefab;
    public int weight = 1;
}

public class MobSpawner : MonoBehaviour
{
    [Header("Mob Types")]
    public List<MobSpawnEntry> mobTypes = new List<MobSpawnEntry>();

    [Header("Spawn Mode")]
    public bool useWaves = true;

    [Header("Wave Settings")]
    public float spawnInterval = 3f;
    public int spawnPerWave = 1;

    [Header("Spawn Settings")]
    public int maxMobs = 10;

    [Header("Spawn Area")]
    public Vector2 areaCenter;
    public Vector2 areaSize = new Vector2(10f, 10f);
    public float minSpawnDistance = 1.5f;

    public event System.Action OnAllMobsCleared;

    private List<GameObject> activeMobs = new List<GameObject>();
    private float spawnTimer;
    private int totalWeight;
    private bool initialSpawnDone;
    private bool hasSpawned;

    private void Start()
    {
        totalWeight = 0;
        foreach (var entry in mobTypes)
            totalWeight += entry.weight;

        PrewarmPool();
    }

    private void PrewarmPool()
    {
        if (MobPool.Instance == null)
            return;

        foreach (var entry in mobTypes)
        {
            int count = Mathf.CeilToInt(maxMobs * ((float)entry.weight / totalWeight));
            MobPool.Instance.Prewarm(entry.prefab, count);
        }
    }

    private void Update()
    {
        activeMobs.RemoveAll(mob => mob == null || !mob.activeInHierarchy);

        if (hasSpawned && activeMobs.Count == 0)
        {
            hasSpawned = false;
            OnAllMobsCleared?.Invoke();
            return;
        }

        if (!useWaves)
        {
            if (!initialSpawnDone)
            {
                SpawnBatch(maxMobs);
                initialSpawnDone = true;
            }
            return;
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            spawnTimer = spawnInterval;
            SpawnBatch(spawnPerWave);
        }
    }

    private void SpawnBatch(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (activeMobs.Count >= maxMobs)
                return;

            GameObject prefab = PickRandomMob();
            if (prefab == null)
                return;

            Vector2 pos = GetRandomPosition();
            GameObject mob = MobPool.Instance.Get(prefab, pos);
            activeMobs.Add(mob);
            hasSpawned = true;
        }
    }

    private GameObject PickRandomMob()
    {
        if (mobTypes.Count == 0 || totalWeight <= 0)
            return null;

        int roll = Random.Range(0, totalWeight);
        foreach (var entry in mobTypes)
        {
            roll -= entry.weight;
            if (roll < 0)
                return entry.prefab;
        }

        return mobTypes[0].prefab;
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 worldCenter = (Vector2)transform.position + areaCenter;
        int maxAttempts = 20;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float x = Random.Range(worldCenter.x - areaSize.x / 2f, worldCenter.x + areaSize.x / 2f);
            float y = Random.Range(worldCenter.y - areaSize.y / 2f, worldCenter.y + areaSize.y / 2f);
            Vector2 candidate = new Vector2(x, y);

            if (!Physics2D.OverlapCircle(candidate, minSpawnDistance))
                return candidate;
        }

        // Fallback if no clear spot found
        float fx = Random.Range(worldCenter.x - areaSize.x / 2f, worldCenter.x + areaSize.x / 2f);
        float fy = Random.Range(worldCenter.y - areaSize.y / 2f, worldCenter.y + areaSize.y / 2f);
        return new Vector2(fx, fy);
    }

    public int ActiveMobCount => activeMobs.Count;

    private void OnDrawGizmosSelected()
    {
        Vector2 worldCenter = (Vector2)transform.position + areaCenter;
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawCube(worldCenter, areaSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(worldCenter, areaSize);
    }
}
