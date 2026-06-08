using System;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerHealth : MonoBehaviour
    {
        // Expose player health with an event
        public event Action<float, float> OnHealthChanged; // (current, max)

        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 3.5f;  // Number of hearts of life
        private float currentHealth;

        [Header("Invulnerability")]
        [SerializeField] private float iFramesDuration = 1f;  // Duration in seconds of invulnerability
        private float iFramesTimer;
        private bool isInvulnerable;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        void Update()
        {
            // Handle invulnerability logic in Update (smoother for non-physical timers)
            HandleInvulnerability();
        }

        private void HandleInvulnerability()
        {
            if (!isInvulnerable) return;

            // Reduce the timer based on real-time passed in the current frame
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
            Debug.Log("Player took damage! Remaining health: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
                return; // Interrupt execution before invulnerability frames if the player is dead
            }

            isInvulnerable = true;
            iFramesTimer = iFramesDuration;
        }

        private void Die()
        {
            // Look for the director on the same object
            if (TryGetComponent(out PlayerEntity playerEntity))
            {
                // Tell the director to handle death (disable controls, start animations, etc.)
                playerEntity.HandlePlayerDeath();
            }
            else
            {
                // Fallback: if there's no manager, destroy the object normally
                Destroy(gameObject);
            }
        }
    }
}
