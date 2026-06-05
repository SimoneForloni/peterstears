using PlayerScripts;
using ScriptableObjects;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(Rigidbody2D))] // Evita bug: Unity aggiungerà automaticamente un Rigidbody2D se manca\
    public class EnemyController : MonoBehaviour
    {
        [Header("Obj Stats")]
        [SerializeField] private EnemyData enemyData;               // Tempo minimo di attesa tra un attacco e l'altro

        private float currentHealth;
        private float nextAttackTime;                                        // Timer per gestire il cooldown degli attacchi

        private Rigidbody2D rb;
        private SpriteRenderer sr;

        private void Start()
        {
            // Recupera il componente Rigidbody2D dal Nemico e lo SpriteRenderer dal figlio
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponentInChildren<SpriteRenderer>();

            if (enemyData)
            {
                currentHealth = enemyData.MaxHealth;

                // Applica lo sprite se presente
                if (sr != null && enemyData.EnemySprite != null)
                {
                    sr.sprite = enemyData.EnemySprite;
                }
                else if (sr == null)
                {
                    Debug.LogError($"ERRORE: Impossibile trovare uno SpriteRenderer nei sotto-oggetti di {gameObject.name}!");
                }
            }
            else
            {
                Debug.LogError($"ATTENZIONE: Manca l'EnemyData assegnato sull'oggetto {gameObject.name}!");
                currentHealth = 3f;
            }


            // Configurazione fisica per un gioco top-down
            rb.gravityScale = 0f;                                            // Niente gravità verso il basso
            rb.freezeRotation = true;                                        // Impedisce al personaggio di ruotare su se stesso quando urta un oggetto
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Per evitare di passare attraverso i muri
        }

        private void FixedUpdate()
        {   
            // Se il player non esiste ritorna
            if (!PlayerEntity.Instance) return;

            // Esegue l'algoritmo di movimento associato allo ScriptableObject dell'AI
            enemyData.AIBehavior.ExecutePhysicsBehavior(this, rb, enemyData);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            // Controlla se l'oggetto con cui sta collidendo è il Player, altrimenti ritorna (TryGetComponent ottimizza la memoria ed evita allocazioni spazzatura)
            if (!collision.gameObject.TryGetComponent(out PlayerEntity player)) return;
            
            // Se non passa abbastanza tempo dall' ultimo attacco ritorna (evita che il danno venga applicato a ogni frame)
            if (Time.time < nextAttackTime) return;
            
            // Applica il danno al giocatore
            player.TakeDamage(enemyData.AttackDamage);

            // Imposta il prossimo momento utile per poter attaccare di nuovo
            nextAttackTime = Time.time + enemyData.AttackCooldown;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"{gameObject.name} ha subito danno! Vita rimasta: {currentHealth}");

            // Se la vita scende a 0 o meno, il nemico muore
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