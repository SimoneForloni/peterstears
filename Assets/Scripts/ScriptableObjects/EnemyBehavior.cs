using Controller;
using UnityEngine;
using PlayerScripts;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyBehavior", menuName = "Scriptable Objects/EnemyBehavior")]
    public abstract class EnemyBehavior : ScriptableObject
    {
        // This method will execute the physics movement logic in the FixedUpdate of the EnemyController
        public abstract void ExecutePhysicsBehavior(EnemyController enemy, Rigidbody2D rb, EnemyData data);
        
        // --- PRE-COMPILED UTIL FUNCTIONS FOR ALL ENEMIES ---
        
        /// <summary>
        /// Checks if the enemy is too close to the player (optimized without square root).
        /// </summary>
        protected bool TooCloseToPlayer(Rigidbody2D rb, float threshold)
        {
            if (!PlayerEntity.Instance) return false;
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            return (playerPos - rb.position).sqrMagnitude < (threshold * threshold);
        }

        /// <summary>
        /// Checks if the enemy is within the correct range relative to the player.
        /// </summary>
        protected bool InTargetRange(Rigidbody2D rb, float minRange, float maxRange)
        {
            if (!PlayerEntity.Instance) return false;
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            var sqrDist = (playerPos - rb.position).sqrMagnitude;
            return sqrDist >= (minRange * minRange) && sqrDist <= (maxRange * maxRange);
        }

        /// <summary>
        /// Physically moves the enemy away from the player.
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
        /// Charges or chases the player head-on.
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
        /// Executes an orbital (strafe) movement perpendicular to the player.
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
