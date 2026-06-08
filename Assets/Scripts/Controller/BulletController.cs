using Pools;
using UnityEngine;

namespace Controller
{
    public class BulletController : MonoBehaviour
    {
        [Header("Bullet Setting")]
        [SerializeField] private float maxRange = 5f; // Maximum distance the bullet can travel

        [Header("Damage")]
        [SerializeField] private float damage = 1f; // Base damage of 1 heart/point

        private Vector2 initialPosition;
        private float maxRangeSquared; // Stores the maximum distance squared for optimized calculations

        void OnEnable()
        {
            // Store the point where the bullet was generated
            initialPosition = transform.position;

            // Calculate the square of the range once at the start.
            // Ex: if maxRange is 5, maxRangeSquared will be 25. Avoids square root calculation in Update.
            maxRangeSquared = maxRange * maxRange;

            Debug.Assert(gameObject.CompareTag("Bullet"), "Missing Bullet tag on prefab!");
        }

        void Update()
        {
            // Calculate the current offset vector relative to the start
            Vector2 currentOffset = (Vector2)transform.position - initialPosition;

            // sqrMagnitude returns the length of the vector SQUARED (without taking the square root)
            // If the distance traveled exceeds the maximum range, the bullet is destroyed
            if (currentOffset.sqrMagnitude >= maxRangeSquared)
            {
                BulletPool.Instance.ReturnBullet(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // if (collision.CompareTag("Wall"))
            // {
            //     // Return the bullet to the pool
            //     BulletPool.Instance.ReturnBullet(gameObject);
            //     return;
            // }

            // If it touches the Player or itself, EXIT IMMEDIATELY and do nothing.
            if (collision.CompareTag("Player") || collision.CompareTag("Bullet")) return;

            // If it doesn't touch an enemy, return. Otherwise, deal damage and destroy it.
            if (!collision.TryGetComponent(out EnemyController enemy)) return;
        
            enemy.TakeDamage(damage);
            BulletPool.Instance.ReturnBullet(gameObject);
            
        }
    }
}
