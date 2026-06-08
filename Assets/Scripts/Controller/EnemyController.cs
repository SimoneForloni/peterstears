using PlayerScripts;
using ScriptableObjects;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(Rigidbody2D))] // Prevents bugs: Unity will automatically add a Rigidbody2D if missing
    public class EnemyController : MonoBehaviour
    {
        [Header("Obj Stats")]
        [SerializeField] private EnemyData enemyData;               // Minimum time between attacks

        private float currentHealth;
        private float nextAttackTime;                                        // Timer to manage attack cooldown

        private Rigidbody2D rb;
        private SpriteRenderer sr;

        private void Start()
        {
            // Get the Rigidbody2D component from the Enemy and the SpriteRenderer from the child
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponentInChildren<SpriteRenderer>();

            if (enemyData)
            {
                currentHealth = enemyData.MaxHealth;

                // Apply sprite if present
                if (sr != null && enemyData.EnemySprite != null)
                {
                    sr.sprite = enemyData.EnemySprite;
                }
                else if (sr == null)
                {
                    Debug.LogError($"ERROR: Unable to find a SpriteRenderer in the child objects of {gameObject.name}!");
                }
            }
            else
            {
                Debug.LogError($"WARNING: No EnemyData assigned on object {gameObject.name}!");
                currentHealth = 3f;
            }


            // Physics configuration for a top-down game
            rb.gravityScale = 0f;                                            // No gravity downwards
            rb.freezeRotation = true;                                        // Prevents rotation when colliding with objects
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Avoid passing through walls
        }

        private void FixedUpdate()
        {   
            // If the player doesn't exist, return
            if (!PlayerEntity.Instance) return;

            // Execute the movement algorithm associated with the AI ScriptableObject
            enemyData.AIBehavior.ExecutePhysicsBehavior(this, rb, enemyData);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            // Check if the colliding object is the Player, otherwise return (TryGetComponent optimizes memory and avoids garbage allocation)
            if (!collision.gameObject.TryGetComponent(out PlayerEntity player)) return;
            
            // If not enough time has passed since the last attack, return (prevents damage from being applied every frame)
            if (Time.time < nextAttackTime) return;
            
            // Apply damage to the player
            player.TakeDamage(enemyData.AttackDamage);

            // Set the next available moment to attack again
            nextAttackTime = Time.time + enemyData.AttackCooldown;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"{gameObject.name} took damage! Remaining health: {currentHealth}");

            // If health drops to 0 or below, the enemy dies
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
