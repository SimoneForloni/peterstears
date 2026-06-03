using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    // Istanza Singleton per essere accessibile da PlayerShooting e BulletController
    public static BulletPool Instance { get; private set; }

    [Header("Configurazione Pool")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 30; // Quanti proiettili pre-istanziare all'inizio

    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializePool();
    }

    private void InitializePool()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Prefab non selezionato");
            return;
        }

        // Creiamo i proiettili da mettere nella pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false); // Disattiva il proiettile cosicche non si veda prima che venga sparato
            poolQueue.Enqueue(bullet);
        }
    }

    public GameObject GetBullet(Vector2 position, Quaternion rotation)
    {
        GameObject bullet;

        // Se la coda ha proiettili disponibili, ne prende uno
        if (poolQueue.Count > 0) bullet = poolQueue.Dequeue();
        // Se finiamo i proiettili (es. rateo di fuoco folle), ne creiamo uno nuovo per sicurezza (espansione dinamica)
        else bullet = Instantiate(bulletPrefab, transform);


        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.SetActive(true); // Attiva il proiettile 

        return bullet;
    }

    // Ripone il proiettile nella coda per poterlo riutilizzare
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false); // Disattiva il proiettile 
        poolQueue.Enqueue(bullet);
    }
}
