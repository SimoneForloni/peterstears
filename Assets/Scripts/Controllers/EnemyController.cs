using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Evita bug: Unity aggiungerà automaticamente un Rigidbody2D se manca\
public class EnemyController : MonoBehaviour
{
    [Header("Obj Stats")]
    [SerializeField] private EnemyData enemyData;               // Tempo minimo di attesa tra un attacco e l'altro

    private float currentHealth;
    private float nextAttackTime;                                        // Timer per gestire il cooldown degli attacchi

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform playerTransform;

    void Start()
    {
        // Recupera il componente Rigidbody2D dal Nemico e lo SpriteRenderer dal figlio
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        if (enemyData != null)
        {
            currentHealth = enemyData.maxHealth;

            // Applica lo sprite se presente
            if (sr != null && enemyData.enemySprite != null)
            {
                sr.sprite = enemyData.enemySprite;
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

        // Controlla che l'istanza della classe PlayerEntity esista (presente solo se il player esiste)
        if (PlayerEntity.Instance != null)
            playerTransform = PlayerEntity.Instance.transform;
    }

    void FixedUpdate()
    {   
        // Se il player non esiste ritorna
        if (playerTransform == null) return;

        // Calcola la direzione verso il giocatore dal centro del Rigidbody
        Vector2 direction = ((Vector2)playerTransform.position - rb.position).normalized;

        // Calcola la nuova posizione target per questo frame basandosi sul fixedDeltaTime
        Vector2 targetPosition = rb.position + direction * enemyData.moveSpeed * Time.fixedDeltaTime;

        // Muove il Rigidbody tenendo conto delle collisioni (evita tremolii, jittering e compenetrazioni nei muri)
        rb.MovePosition(targetPosition);
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Controlla se l'oggetto con cui sta collidendo è il Player (TryGetComponent ottimizza la memoria ed evita allocazioni spazzatura)
        if (collision.gameObject.TryGetComponent<PlayerEntity>(out PlayerEntity player))
        {
            // Attacca solo se è passato abbastanza tempo dall'ultimo colpo (evita che il danno venga applicato ad ogni frame)
            if (Time.time >= nextAttackTime)
            {
                // Applica il danno al giocatore
                player.Health.TakeDamage(enemyData.attackDamage);

                // Imposta il prossimo momento utile per poter attaccare di nuovo
                nextAttackTime = Time.time + enemyData.attackCooldown;
            }
        }
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
        // Qui potresti spawnare particelle o rilasciare monete/esperienza
        Destroy(gameObject);
    }
}