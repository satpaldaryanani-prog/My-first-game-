using UnityEngine;
using System.Collections.Generic;

namespace CatFootball.Data
{
    /// <summary>
    /// ScriptableObject defining a cat character's stats and properties
    /// Designers can create character assets without touching code
    /// </summary>
    [CreateAssetMenu(fileName = "New Cat Character", menuName = "Cat Football/Character Data")]
    public class CatCharacterData : ScriptableObject
    {
        [Header("Basic Info")]
        public string characterName = "Whiskers";
        public string characterID = "cat_001";

        [TextArea(3, 5)]
        public string description = "A friendly orange tabby who loves football";

        public Rarity rarity = Rarity.Common;
        public CharacterPosition position = CharacterPosition.Midfielder;

        [Header("Visuals")]
        public Sprite characterSprite;
        public Sprite cardArtwork;
        public RuntimeAnimatorController animatorController;
        public Color primaryColor = Color.white;

        [Header("Cat Stats")]
        [Range(Constants.STAT_MIN, Constants.STAT_MAX)]
        public float pouncePower = 50f; // Shooting strength

        [Range(Constants.STAT_MIN, Constants.STAT_MAX)]
        public float whiskerPrecision = 50f; // Passing accuracy

        [Range(Constants.STAT_MIN, Constants.STAT_MAX)]
        public float tailBalance = 50f; // Dribbling/agility

        [Range(Constants.STAT_MIN, Constants.STAT_MAX)]
        public float clawGrip = 50f; // Ball control

        [Range(Constants.STAT_MIN, Constants.STAT_MAX)]
        public float nineLives = 50f; // Stamina

        [Range(Constants.STAT_MIN, Constants.STAT_MAX)]
        public float purrsuasion = 50f; // Crowd effect bonus

        [Header("Personality")]
        public PersonalityTrait personality = PersonalityTrait.Balanced;

        [Range(1, 5)]
        public int playfulness = 3;

        [Range(1, 5)]
        public int laziness = 3;

        [Range(1, 5)]
        public int competitiveness = 3;

        [Range(1, 5)]
        public int scaredyCatLevel = 3;

        [Header("Evolution")]
        public EvolutionTier currentTier = EvolutionTier.Kitten;
        public int maxEvolutionTiers = 2; // Common: 2, Rare: 3, Epic/Legendary: 4
        public CatCharacterData nextEvolution;

        [Header("Abilities")]
        public List<AbilityData> abilities = new List<AbilityData>();

        [Header("Unlock Requirements")]
        public int unlockLevel = 1;
        public int fishCoinsRequired = 0;
        public int catnipGemsRequired = 0;

        /// <summary>
        /// Calculate overall rating based on stats
        /// </summary>
        public float GetOverallRating()
        {
            return (pouncePower + whiskerPrecision + tailBalance + clawGrip + nineLives + purrsuasion) / 6f;
        }

        /// <summary>
        /// Get stat by name (for dynamic access)
        /// </summary>
        public float GetStat(string statName)
        {
            return statName switch
            {
                "pouncePower" => pouncePower,
                "whiskerPrecision" => whiskerPrecision,
                "tailBalance" => tailBalance,
                "clawGrip" => clawGrip,
                "nineLives" => nineLives,
                "purrsuasion" => purrsuasion,
                _ => 0f
            };
        }

        /// <summary>
        /// Check if character can evolve
        /// </summary>
        public bool CanEvolve()
        {
            return nextEvolution != null && (int)currentTier < maxEvolutionTiers - 1;
        }

        /// <summary>
        /// Get rarity color for UI
        /// </summary>
        public Color GetRarityColor()
        {
            return rarity switch
            {
                Rarity.Common => new Color(0.7f, 0.7f, 0.7f), // Grey
                Rarity.Rare => new Color(0.2f, 0.5f, 1f), // Blue
                Rarity.Epic => new Color(0.6f, 0.2f, 1f), // Purple
                Rarity.Legendary => new Color(1f, 0.8f, 0f), // Gold
                _ => Color.white
            };
        }
    }
}
