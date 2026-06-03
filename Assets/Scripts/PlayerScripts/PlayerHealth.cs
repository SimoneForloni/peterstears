using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Espone la vita del player con un evento
    public event Action<float, float> OnHealthChanged; // (current, max)

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 3.5f;  // Numero di cuori di vita
    private float currentHealth;

    [Header("Invulnerability")]
    [SerializeField] private float iFramesDuration = 1f;  // Tempo in secondi di invulnerabilità
    private float iFramesTimer;
    private bool isInvulnerable = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Logica dei frame di invulnerabilità gestita nell'Update (più fluida per i timer non fisici)
        HandleInvulnerability();
    }

    private void HandleInvulnerability()
    {
        if (!isInvulnerable) return;

        // Riduce il timer in base al tempo reale passato nel frame corrente
        iFramesTimer -= Time.deltaTime;
        if (iFramesTimer <= 0)
        {
            isInvulnerable = false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Player ha subito danno! Vita rimasta: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return; // Interrompe l'esecuzione prima dei frame di invulnerabilità se il player è morto
        }

        isInvulnerable = true;
        iFramesTimer = iFramesDuration;
    }

    private void Die()
    {
        // Cerca il direttore d'orchestra sullo stesso oggetto
        if (TryGetComponent<PlayerEntity>(out PlayerEntity playerEntity))
        {
            // Dice al direttore di gestire la morte (disattivare controlli, far partire animazioni, ecc.)
            playerEntity.HandlePlayerDeath();
        }
        else
        {
            // Fallback di sicurezza: se non c'è il manager, distrugge l'oggetto normalmente
            Destroy(gameObject);
        }
    }
}
