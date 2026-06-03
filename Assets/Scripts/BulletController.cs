using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Impostazioni del proiettile")]
    [SerializeField] private float maxRange = 5f; // Distanza massima che il proiettile può percorrere

    [Header("Danno")]
    [SerializeField] private float damage = 1f; // Di base fa 1 cuore/punto di danno

    private Vector2 initialPosition;
    private float maxRangeSquared; // Memorizza la distanza massima al quadrato per ottimizzare i calcoli

    void OnEnable()
    {
        // Memorizza il punto in cui il proiettile è stato generato
        initialPosition = transform.position;

        // Calcoliamo il quadrato del range una volta sola all'inizio.
        // Es: se maxRange è 5, maxRangeSquared sarà 25. Evita il calcolo della radice quadrata nell'Update.
        maxRangeSquared = maxRange * maxRange;

        Debug.Assert(gameObject.CompareTag("PlayerBullet"), "Manca il tag PlayerBullet sul prefab!");
    }

    void Update()
    {
        // Calcola il vettore di spostamento attuale rispetto alla partenza
        Vector2 currentOffset = (Vector2)transform.position - initialPosition;

        // sqrMagnitude restituisce la lunghezza del vettore AL QUADRATO (senza fare la radice)
        // Se la distanza percorsa supera il range massimo, il proiettile viene distrutto
        if (currentOffset.sqrMagnitude >= maxRangeSquared)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se tocca il Player (o se stesso), ESCE SUBITO e non fa nulla
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerBullet"))
        {
            return;
        }

        // Se tocca il nemico, fa danno e si distrugge
        if (collision.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}