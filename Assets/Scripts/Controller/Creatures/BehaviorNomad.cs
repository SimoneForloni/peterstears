using PlayerScripts;
using ScriptableObjects;
using UnityEngine;

namespace Controller.Creatures
{
    public class BehaviorNomad : EnemyBehavior
    {
        public override void ExecutePhysicsBehavior(EnemyController enemy, Rigidbody2D rb, EnemyData data)
        {
            if (!PlayerEntity.Instance) return;
            
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            var directionToPlayer = playerPos - rb.position;
            var distance  = directionToPlayer.magnitude;
            var normalizedDirection = directionToPlayer.normalized;

            Vector2 targetDirection;

            if (distance > data.AttackRange + 2f)
            {
                targetDirection = normalizedDirection;
            }
            else if (distance <= data.AttackRange + 2f && distance > +data.AttackRange - 2f)
            {
                targetDirection = Vector2.Perpendicular(normalizedDirection);
            }
            else
            {
                targetDirection = -normalizedDirection;
            }
            
            var targetPosition = (Vector2)rb.transform.position + targetDirection * (data.MoveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(targetPosition);
        }
    }
}