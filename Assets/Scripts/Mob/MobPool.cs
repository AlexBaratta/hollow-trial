using System.Collections.Generic;
using UnityEngine;

public class MobPool : MonoBehaviour
{
    public static MobPool Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, GameObject> instanceToPrefab = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Prewarm(GameObject prefab, int count)
    {
        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool[prefab].Enqueue(obj);
            instanceToPrefab[obj] = prefab;
        }
    }

    public GameObject Get(GameObject prefab, Vector2 position)
    {
        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<GameObject>();

        GameObject obj;
        if (pool[prefab].Count > 0)
        {
            obj = pool[prefab].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = Quaternion.identity;
        }
        else
        {
            obj = Instantiate(prefab, position, Quaternion.identity, transform);
            instanceToPrefab[obj] = prefab;
        }

        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (!instanceToPrefab.TryGetValue(obj, out GameObject prefab))
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        pool[prefab].Enqueue(obj);
    }
}
