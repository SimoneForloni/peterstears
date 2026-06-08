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
        [field: SerializeField] public GeneticStrain Strain { get; private set; } // The genetic strain of this species
        [field: SerializeField] public GameObject BiomassPrefab { get; private set; } // The prefab for the biomass/gene that is dropped
        [field: SerializeField] public int BiomassRewardAmount { get; private set; } = 3; // How much biomass is dropped on death

        [field: Header("Combat Stats")] 
        [field: SerializeField] public float MaxHealth { get; private set; } = 3f;
        [field: SerializeField] public float AttackDamage { get; private set; } = 0.5f;
        [field: SerializeField] public float AttackCooldown { get; private set; } = 1f;

        [field: Header("Move Stats")] 
        [field: SerializeField] public float MoveSpeed { get; private set; } = 3f;
        
        [field: Header("AI Configuration")]
        [field: SerializeField] public EnemyBehavior AIBehavior { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; } = 1.5f; // Optimal distance for attack/kiting behaviors
    }
}
