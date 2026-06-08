using Controller;
using UnityEngine;
using PlayerScripts;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyBehavior", menuName = "Scriptable Objects/EnemyBehavior")]
    public abstract class EnemyBehavior : ScriptableObject
    {
        // Questo metodo eseguirà la logica di movimento fisica nel FixedUpdate dell' EnemyController
        public abstract void ExecutePhysicsBehavior(EnemyController enemy, Rigidbody2D rb, EnemyData data);
        
        // --- FUNZIONI UTILI PRE-COMPILATE PER TUTTI I NEMICI ---
        
        /// <summary>
        /// Verifica se il nemico è troppo vicino al giocatore (ottimizzato senza radice quadrata).
        /// </summary>
        protected bool TooCloseToPlayer(Rigidbody2D rb, float threshold)
        {
            if (!PlayerEntity.Instance) return false;
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            return (playerPos - rb.position).sqrMagnitude < (threshold * threshold);
        }

        /// <summary>
        /// Verifica se il nemico si trova nel range corretto rispetto al giocatore.
        /// </summary>
        protected bool InTargetRange(Rigidbody2D rb, float minRange, float maxRange)
        {
            if (!PlayerEntity.Instance) return false;
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            var sqrDist = (playerPos - rb.position).sqrMagnitude;
            return sqrDist >= (minRange * minRange) && sqrDist <= (maxRange * maxRange);
        }

        /// <summary>
        /// Muove fisicamente il nemico nella direzione opposta al giocatore.
        /// </summary>
        protected void RunAway(Rigidbody2D rb, float speed)
        {
            if (!PlayerEntity.Instance) return;
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            var directionFromPlayer = (rb.position - playerPos).normalized;
            var targetPosition = rb.position + directionFromPlayer * (speed * Time.fixedDeltaTime);
            rb.MovePosition(targetPosition);
        }

        /// <summary>
        /// Carica o insegue fisicamente il giocatore a testa bassa.
        /// </summary>
        protected void ChargePlayer(Rigidbody2D rb, float speed)
        {
            if (!PlayerEntity.Instance) return;
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            var directionToPlayer = (playerPos - rb.position).normalized;
            var targetPosition = rb.position + directionToPlayer * (speed * Time.fixedDeltaTime);
            rb.MovePosition(targetPosition);
        }

        /// <summary>
        /// Esegue un movimento orbitale (strafe) perpendicolare al giocatore.
        /// </summary>
        protected void OrbitPlayer(Rigidbody2D rb, float speed, bool clockwise = true)
        {
            if (!PlayerEntity.Instance) return;
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            var directionToPlayer = (playerPos - rb.position).normalized;
            var orbitDirection = clockwise ? Vector2.Perpendicular(directionToPlayer) : -Vector2.Perpendicular(directionToPlayer);
            var targetPosition = rb.position + orbitDirection * (speed * Time.fixedDeltaTime);
            rb.MovePosition(targetPosition);
        }
    }
}
