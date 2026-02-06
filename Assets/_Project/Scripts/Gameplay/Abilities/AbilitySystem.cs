using UnityEngine;
using System.Collections.Generic;
using CatFootball.Data;

namespace CatFootball.Gameplay
{
    /// <summary>
    /// Manages character abilities and cooldowns
    /// </summary>
    public class AbilitySystem : MonoBehaviour
    {
        [Header("Abilities")]
        [SerializeField] private List<AbilityData> availableAbilities = new List<AbilityData>();

        private Dictionary<AbilityType, float> abilityCooldowns = new Dictionary<AbilityType, float>();
        private Dictionary<AbilityType, bool> abilityReady = new Dictionary<AbilityType, bool>();

        private void Start()
        {
            InitializeAbilities();
        }

        private void Update()
        {
            UpdateCooldowns();
        }

        private void InitializeAbilities()
        {
            foreach (var ability in availableAbilities)
            {
                if (ability != null)
                {
                    abilityCooldowns[ability.abilityType] = 0f;
                    abilityReady[ability.abilityType] = true;
                }
            }

            Debug.Log($"AbilitySystem initialized with {availableAbilities.Count} abilities");
        }

        private void UpdateCooldowns()
        {
            List<AbilityType> keys = new List<AbilityType>(abilityCooldowns.Keys);

            foreach (AbilityType abilityType in keys)
            {
                if (abilityCooldowns[abilityType] > 0f)
                {
                    abilityCooldowns[abilityType] -= Time.deltaTime;

                    if (abilityCooldowns[abilityType] <= 0f)
                    {
                        abilityCooldowns[abilityType] = 0f;
                        abilityReady[abilityType] = true;

                        Debug.Log($"Ability {abilityType} is ready!");
                    }
                }
            }
        }

        /// <summary>
        /// Attempt to use an ability
        /// </summary>
        public bool UseAbility(AbilityType abilityType)
        {
            if (!IsAbilityReady(abilityType))
            {
                Debug.Log($"Ability {abilityType} is on cooldown");
                return false;
            }

            AbilityData abilityData = GetAbilityData(abilityType);
            if (abilityData == null)
            {
                Debug.LogWarning($"Ability {abilityType} not found!");
                return false;
            }

            // Execute ability
            ExecuteAbility(abilityData);

            // Start cooldown
            abilityCooldowns[abilityType] = abilityData.cooldownTime;
            abilityReady[abilityType] = false;

            return true;
        }

        private void ExecuteAbility(AbilityData ability)
        {
            // Ability-specific logic
            switch (ability.abilityType)
            {
                case AbilityType.PounceStrike:
                    ExecutePounceStrike(ability);
                    break;

                case AbilityType.CatnipBoost:
                    ExecuteCatnipBoost(ability);
                    break;

                case AbilityType.YarnBallCurve:
                    ExecuteYarnBallCurve(ability);
                    break;

                case AbilityType.FurryFury:
                    ExecuteFurryFury(ability);
                    break;

                default:
                    Debug.Log($"Ability {ability.abilityType} executed (base implementation)");
                    break;
            }

            // Spawn visual effect
            if (ability.effectPrefab != null)
            {
                GameObject effect = Instantiate(ability.effectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, ability.duration);
            }

            // Play sound
            if (ability.activationSound != null)
            {
                // AudioManager.Instance.PlaySFX(ability.activationSound);
            }
        }

        private void ExecutePounceStrike(AbilityData ability)
        {
            // Quick dash to ball
            Debug.Log("Pounce Strike executed!");
            // TODO: Implement dash mechanic
        }

        private void ExecuteCatnipBoost(AbilityData ability)
        {
            // Speed boost
            Debug.Log("Catnip Boost executed!");
            // TODO: Apply speed buff for duration
        }

        private void ExecuteYarnBallCurve(AbilityData ability)
        {
            // Curve shot
            Debug.Log("Yarn Ball Curve executed!");
            // TODO: Apply curve to next shot
        }

        private void ExecuteFurryFury(AbilityData ability)
        {
            // Power shot
            Debug.Log("Furry Fury executed!");
            // TODO: Power up next shot
        }

        /// <summary>
        /// Check if ability is ready
        /// </summary>
        public bool IsAbilityReady(AbilityType abilityType)
        {
            return abilityReady.ContainsKey(abilityType) && abilityReady[abilityType];
        }

        /// <summary>
        /// Get remaining cooldown time
        /// </summary>
        public float GetCooldownRemaining(AbilityType abilityType)
        {
            return abilityCooldowns.ContainsKey(abilityType) ? abilityCooldowns[abilityType] : 0f;
        }

        /// <summary>
        /// Get cooldown progress (0-1, where 1 is ready)
        /// </summary>
        public float GetCooldownProgress(AbilityType abilityType)
        {
            AbilityData data = GetAbilityData(abilityType);
            if (data == null) return 0f;

            float remaining = GetCooldownRemaining(abilityType);
            return 1f - (remaining / data.cooldownTime);
        }

        private AbilityData GetAbilityData(AbilityType abilityType)
        {
            return availableAbilities.Find(a => a != null && a.abilityType == abilityType);
        }

        /// <summary>
        /// Add ability to available abilities
        /// </summary>
        public void AddAbility(AbilityData ability)
        {
            if (ability != null && !availableAbilities.Contains(ability))
            {
                availableAbilities.Add(ability);
                abilityCooldowns[ability.abilityType] = 0f;
                abilityReady[ability.abilityType] = true;

                Debug.Log($"Ability {ability.abilityType} added");
            }
        }

        /// <summary>
        /// Get all available abilities
        /// </summary>
        public List<AbilityData> GetAvailableAbilities()
        {
            return new List<AbilityData>(availableAbilities);
        }
    }
}
