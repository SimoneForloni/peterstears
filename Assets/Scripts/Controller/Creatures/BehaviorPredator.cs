using PlayerScripts;
using ScriptableObjects;
using UnityEngine;

namespace Controller.Creatures
{
    public class BehaviorPredator : EnemyBehavior
    {
        public override void ExecutePhysicsBehavior(EnemyController enemy, Rigidbody2D rb, EnemyData data)
        {
            if (!PlayerEntity.Instance) return;
           
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            var direction = playerPos - (Vector2)rb.transform.position;
            var targetPosition = (Vector2)rb.transform.position + direction * (data.MoveSpeed * Time.fixedDeltaTime);
            
            rb.MovePosition(targetPosition);
        }
    }
}