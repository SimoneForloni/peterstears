using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerShooting))]
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerEntity : MonoBehaviour
    {
        // Istanza del player (questa classe)
        public static PlayerEntity Instance { get; private set; }

        // Riferimenti ai sotto-moduli (accessibili in sola lettura dall'esterno)
        private PlayerInputHandler inputHandler;
        private PlayerMovement movement;
        private PlayerShooting shooting;
        private PlayerHealth health;

        public void TakeDamage(float damage)
        {
            health.TakeDamage(damage);
        }
        

        private void Awake()
        {
            // Assegna l'istanza di questa classe alla variabile
            Instance = this;
            // Centralizziamo il recupero di tutti i componenti sul Player
            inputHandler = GetComponent<PlayerInputHandler>();
            movement = GetComponent<PlayerMovement>();
            shooting = GetComponent<PlayerShooting>();
            health = GetComponent<PlayerHealth>();
        }

        // Esempio di gestione centrale di un evento: il Player muore
        public void HandlePlayerDeath()
        {
            Debug.Log("Il Direttore d'Orchestra disattiva il Player.");
        
            // Disattiviamo l'input e lo sparo prima di distruggere l'oggetto
            inputHandler.enabled = false;
            shooting.enabled = false;
            movement.enabled = false;

            Destroy(gameObject, 0.5f); // Distruggi dopo mezzo secondo (magari per far finire un'animazione)
        }
    }
}