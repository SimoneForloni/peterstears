using PlayerScripts;
using ScriptableObjects;
using UnityEngine;

namespace Controller.Creatures
{
    public class BehaviorSniper : EnemyBehavior
    {
        public override void ExecutePhysicsBehavior(EnemyController enemy, Rigidbody2D rb, EnemyData data)
        {
            if (!PlayerEntity.Instance) return;
                
            Vector2 playerPos = PlayerEntity.Instance.transform.position;
            var direction = playerPos - rb.position;
            var distance = direction.magnitude;
            
            // Se il giocatore è troppo vicino, scappa per riposizionarsi
            if (distance < data.AttackRange * 0.6f)
            {
                var escapePos = rb.position - direction.normalized * (data.MoveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(escapePos);
            }
            // Se è troppo lontano, si avvicina fino al suo raggio d'azione ottimale
            else if (distance > data.AttackRange)
            {
                var approachPos = rb.position + direction.normalized * (data.MoveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(approachPos);
            }
            // Se si trova nel raggio d'azione ideale, azzera la velocità fisica e spara (la logica di sparo sarà gestita dal controller)
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}