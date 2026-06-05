using Controller;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyBehavior", menuName = "Scriptable Objects/EnemyBehavior")]
    public abstract class EnemyBehavior : ScriptableObject
    {
        // Questo metodo eseguirà la logica di movimento fisica nel FixedUpdate dell' EnemyController
        public abstract void ExecutePhysicsBehavior(EnemyController enemy, Rigidbody2D rb, EnemyData data);
    }
}
