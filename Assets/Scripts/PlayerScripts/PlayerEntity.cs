using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerShooting))]
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerEntity : MonoBehaviour
    {
        // Instance of the player (this class)
        public static PlayerEntity Instance { get; private set; }

        // References to sub-modules (read-only from outside)
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
            // Assign this class instance to the variable
            Instance = this;
            // Centralize retrieval of all player components
            inputHandler = GetComponent<PlayerInputHandler>();
            movement = GetComponent<PlayerMovement>();
            shooting = GetComponent<PlayerShooting>();
            health = GetComponent<PlayerHealth>();
        }

        // Example of centralized event handling: Player death
        public void HandlePlayerDeath()
        {
            Debug.Log("The Director deactivates the Player.");
        
            // Disable input and shooting before destroying the object
            inputHandler.enabled = false;
            shooting.enabled = false;
            movement.enabled = false;

            Destroy(gameObject, 0.5f); // Destroy after half a second (maybe to finish an animation)
        }
    }
}
