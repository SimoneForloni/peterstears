using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
    public class BulletPool : MonoBehaviour
    {
        // Singleton instance to be accessible by PlayerShooting and BulletController
        public static BulletPool Instance { get; private set; }

        [Header("Configurazione Pool")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private int poolSize = 30; // Number of bullets to pre-instantiate at the start

        private readonly Queue<GameObject> poolQueue = new();

        void Awake()
        {
            if (!Instance) Instance = this;
            else Destroy(gameObject);

            InitializePool();
        }

        private void InitializePool()
        {
            if (bulletPrefab == null)
            {
                Debug.LogError("Prefab not selected");
                return;
            }

            // Create bullets to put in the pool
            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform);
                bullet.SetActive(false); // Deactivate the bullet so it's not visible before being shot
                poolQueue.Enqueue(bullet);
            }
        }

        public GameObject GetBullet(Vector2 position, Quaternion rotation)
        {
            // Create a bullet
            // If the queue has available bullets, take one
            // If we run out of bullets (e.g., high fire rate), create a new one for safety (dynamic expansion)
            GameObject bullet = (poolQueue.Count > 0) ? poolQueue.Dequeue() : Instantiate(bulletPrefab, transform);

            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.SetActive(true); // Activate the bullet 

            return bullet;
        }

        // Return the bullet to the queue for reuse
        public void ReturnBullet(GameObject bullet)
        {
            bullet.SetActive(false); // Deactivate the bullet 
            poolQueue.Enqueue(bullet);
        }
    }
}
