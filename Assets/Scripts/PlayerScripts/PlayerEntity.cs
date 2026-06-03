using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShooting))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerEntity : MonoBehaviour
{
    // Riferimenti ai sotto-moduli (accessibili in sola lettura dall'esterno)
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerShooting Shooting { get; private set; }
    public PlayerHealth Health { get; private set; }

    private void Awake()
    {
        // Centralizziamo il recupero di tutti i componenti sul Player
        InputHandler = GetComponent<PlayerInputHandler>();
        Movement = GetComponent<PlayerMovement>();
        Shooting = GetComponent<PlayerShooting>();
        Health = GetComponent<PlayerHealth>();
    }

    // Esempio di gestione centrale di un evento: il Player muore
    public void HandlePlayerDeath()
    {
        Debug.Log("Il Direttore d'Orchestra disattiva il Player.");
        
        // Disattiviamo l'input e lo sparo prima di distruggere l'oggetto
        InputHandler.enabled = false;
        Shooting.enabled = false;
        Movement.enabled = false;

        Destroy(gameObject, 0.5f); // Distruggi dopo mezzo secondo (magari per far finire un'animazione)
    }
}