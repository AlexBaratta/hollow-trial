using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;
    public int poolSize = 20;

    private float nextFireTime = 0f;
    private Camera mainCamera;
    private Queue<GameObject> pool;

    void Awake()
    {
        mainCamera = Camera.main;

        pool = new Queue<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;

        Vector2 direction = (mouseWorldPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject projectile = GetFromPool();
        projectile.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, 0f, angle - 90f));
        projectile.SetActive(true);
        projectile.GetComponent<Projectile>().Launch(direction, this);
    }

    GameObject GetFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            GameObject obj = pool.Dequeue();
            if (!obj.activeInHierarchy)
                return obj;
            pool.Enqueue(obj);
        }

        // grow pool if exhausted
        GameObject newObj = Instantiate(projectilePrefab);
        newObj.SetActive(false);
        pool.Enqueue(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
