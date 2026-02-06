using UnityEngine;

namespace CatFootball.Data
{
    /// <summary>
    /// ScriptableObject defining a special ability
    /// </summary>
    [CreateAssetMenu(fileName = "New Ability", menuName = "Cat Football/Ability Data")]
    public class AbilityData : ScriptableObject
    {
        [Header("Basic Info")]
        public string abilityName = "Pounce Strike";
        public string abilityID = "ability_001";

        [TextArea(2, 4)]
        public string description = "Quick dash to the ball";

        public AbilityType abilityType = AbilityType.PounceStrike;

        [Header("Mechanics")]
        public float cooldownTime = 15f;
        public float duration = 0.5f;
        public float effectValue = 1.5f; // Multiplier or specific value depending on ability

        [Header("Costs")]
        public int catnipBoostCost = 0; // Some abilities may cost boosts
        public float staminaCost = 10f;

        [Header("Visual Effects")]
        public GameObject effectPrefab;
        public Sprite iconSprite;
        public Color effectColor = Color.white;
        public AnimationClip abilityAnimation;

        [Header("Audio")]
        public AudioClip activationSound;
        public AudioClip impactSound;

        [Header("Requirements")]
        public Rarity minimumRarity = Rarity.Common;
        public int unlockLevel = 1;
    }
}
