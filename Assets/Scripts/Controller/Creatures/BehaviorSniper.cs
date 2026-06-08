using PlayerScripts;
using ScriptableObjects;
using UnityEngine;

namespace Controller.Creatures
{
    public class BehaviorSniper : EnemyBehavior
    {
        // Executes the physics-based behavior for the sniper enemy.
        public override void ExecutePhysicsBehavior(EnemyController enemy, Rigidbody2D rb, EnemyData data)
        {
            if (!PlayerEntity.Instance) return;
                
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            var direction = playerPos - rb.position;
            var distance = direction.magnitude;
            
            // If the player is too close, escape to reposition
            if (distance < data.AttackRange * 0.6f)
            {
                var escapePos = rb.position - direction.normalized * (data.MoveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(escapePos);
            }
            // If it's too far, approach until optimal attack range
            else if (distance > data.AttackRange)
            {
                var approachPos = rb.position + direction.normalized * (data.MoveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(approachPos);
            }
            // If in the ideal attack range, zero out physical velocity and shoot (shooting logic will be handled by the controller)
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
