using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab; // Prefab del proiettile
    [SerializeField] private float fireRate = 0.1f;   // Tempo di attesa tra ogni colpo
    [SerializeField] private float bulletSpeed = 10f; // Velocità del proiettile

    private PlayerInputHandler inputHandler;
    private Camera mainCamera;
    private float nextFireTime;

    private void Awake()
    {
        // Recupera il componente InputHandler dal Player
        inputHandler = GetComponent<PlayerInputHandler>();

        // Caching della camera principale per evitare l'uso dispendioso di Camera.main nell'Update
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Logica dello sparo
        HandleShooting();
    }

    private void HandleShooting()
    {
        // Se la mainCamera viene distrutta viene ricreata qua (es. Cambio di scena)
        if (mainCamera == null) mainCamera = Camera.main;
        // Se è passato abbastanza tempo procede con la logica di sparo del proiettile
        if (!inputHandler.IsShootPressed || Time.time < nextFireTime) return;

        // Se il pointer esiste ottieni la posizione del mouse generica(funziona sia con Mouse che con Touch/Pointer su più piattaforme)
        if (Pointer.current == null) return;
        Vector2 mouseWindowPos = Pointer.current.position.ReadValue();

        // Calcola la direzione dal centro dello schermo (dove c'è il player) verso il cursore usando la camera in cache
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseWindowPos);
        mouseWorldPosition.z = 0f;   // La z (profondità) non serve

        Vector2 shootDirection = ((Vector2)mouseWorldPosition - (Vector2)transform.position).normalized;

        // Logica dei proiettili
        ShootBullet(shootDirection);
        nextFireTime = Time.time + fireRate;    // Resetta il timer di ricarica
    }

    private void ShootBullet(Vector2 direction)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Prefab proiettile mancante");
            return;
        }

        // Aggiunge un offset allo spawn del proiettile per evitare collisioni con il player
        Vector2 spawnPos = (Vector2)transform.position + direction * 0.3f;

        // Crea il proiettile nella posizione attuale del giocatore
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        // Assegna la velocità al proiettile (TryGetComponent ottimizza la memoria ed evita allocazioni spazzatura)
        if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D bulletRb))
        {
            bulletRb.linearVelocity = direction * bulletSpeed;
        }
    }
}
