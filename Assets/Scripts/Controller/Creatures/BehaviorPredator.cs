using PlayerScripts;
using ScriptableObjects;
using UnityEngine;

namespace Controller.Creatures
{
    public class BehaviorPredator : EnemyBehavior
    {
        // Executes the physics-based behavior for the predator enemy.
        public override void ExecutePhysicsBehavior(EnemyController enemy, Rigidbody2D rb, EnemyData data)
        {
            if (!PlayerEntity.Instance) return;
           
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            var direction = playerPos - rb.position;
            var targetPosition = (Vector2)rb.transform.position + direction * (data.MoveSpeed * Time.fixedDeltaTime);
            
            rb.MovePosition(targetPosition);
        }
    }
}
