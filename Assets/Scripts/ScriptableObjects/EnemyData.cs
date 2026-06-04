using UnityEngine;

// Questo attributo fa apparire la voce nel menu del tasto destro di Unity
[CreateAssetMenu(fileName = "BaseEnemyData", menuName = "Gamedata/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Identity")]
    public string enemyName = "BaseEnemy";
    public Sprite enemySprite;

    [Header("Combat Stats")]
    public float maxHealth = 3f;
    public float attackDamage = 0.5f;
    public float attackCooldown = 1f;

    [Header("Move Stats")]
    public float moveSpeed = 3f;
}
