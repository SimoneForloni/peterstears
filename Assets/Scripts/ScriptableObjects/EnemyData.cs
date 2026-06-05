using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "BaseEnemyData", menuName = "Gamedata/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [field: Header("Identity")] 
        [field: SerializeField] public string EnemyName { get; private set; } = "BaseEnemy";
        [field: SerializeField] public Sprite EnemySprite { get; private set; }

        [field: Header("Evolution & Drops")]
        [field: SerializeField] public GeneticStrain Strain { get; private set; } // Il ceppo genetico di questa specie
        [field: SerializeField] public GameObject BiomassPrefab { get; private set; } // Il prefab della biomassa/gene che rilascia
        [field: SerializeField] public int BiomassRewardAmount { get; private set; } = 3; // Quanta biomassa rilascia alla morte

        [field: Header("Combat Stats")] 
        [field: SerializeField] public float MaxHealth { get; private set; } = 3f;
        [field: SerializeField] public float AttackDamage { get; private set; } = 0.5f;
        [field: SerializeField] public float AttackCooldown { get; private set; } = 1f;

        [field: Header("Move Stats")] 
        [field: SerializeField] public float MoveSpeed { get; private set; } = 3f;
        
        [field: Header("AI Configuration")]
        [field: SerializeField] public EnemyBehavior AIBehavior { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; } = 1.5f; // Distanza ottimale per i comportamenti di attacco/kiting
    }
}