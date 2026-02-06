using UnityEngine;
using UnityEngine.Events;

namespace CatFootball.Gameplay
{
    /// <summary>
    /// Manages the Meowmentum Chain System
    /// Rewards skillful play sequences with progressive team boosts
    /// </summary>
    public class MeowmentumManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool enabled = true;
        [SerializeField] private float chainDecayTime = 5f; // Time before chain starts decaying

        [Header("State")]
        [SerializeField] private int currentChain = 0;
        [SerializeField] private MeowmentumLevel currentLevel = MeowmentumLevel.None;
        [SerializeField] private float timeSinceLastChain = 0f;

        [Header("Effects")]
        [SerializeField] private GameObject[] levelEffects = new GameObject[5];

        // Events
        public UnityEvent<int> OnChainChanged;
        public UnityEvent<MeowmentumLevel> OnLevelChanged;
        public UnityEvent<int, MeowmentumLevel> OnChainBuilt;

        // Bonuses per level
        private readonly float[] speedBonuses = { 0f, 0.05f, 0.10f, 0.15f, 0.20f, 0.25f };
        private readonly float[] statBonuses = { 0f, 0.05f, 0.10f, 0.15f, 0.20f, 0.25f };

        private void Update()
        {
            if (!enabled) return;

            UpdateChainDecay();
        }

        /// <summary>
        /// Add chain points for successful actions
        /// </summary>
        public void AddChainPoints(int points, string action = "")
        {
            if (!enabled) return;

            currentChain += points;
            timeSinceLastChain = 0f;

            MeowmentumLevel newLevel = CalculateLevel(currentChain);

            if (newLevel != currentLevel)
            {
                LevelUp(newLevel);
            }

            OnChainChanged?.Invoke(currentChain);
            OnChainBuilt?.Invoke(currentChain, currentLevel);

            Debug.Log($"Meowmentum +{points} from {action}! Chain: {currentChain}, Level: {currentLevel}");
        }

        /// <summary>
        /// Reset chain (called when opponent tackles or scores)
        /// </summary>
        public void ResetChain()
        {
            currentChain = 0;
            timeSinceLastChain = 0f;

            if (currentLevel != MeowmentumLevel.None)
            {
                LevelDown();
            }

            OnChainChanged?.Invoke(currentChain);

            Debug.Log("Meowmentum chain broken!");
        }

        /// <summary>
        /// Reduce chain (partial penalty)
        /// </summary>
        public void ReduceChain(int amount)
        {
            currentChain = Mathf.Max(0, currentChain - amount);

            MeowmentumLevel newLevel = CalculateLevel(currentChain);

            if (newLevel != currentLevel)
            {
                currentLevel = newLevel;
                UpdateEffects();
                OnLevelChanged?.Invoke(currentLevel);
            }

            OnChainChanged?.Invoke(currentChain);

            Debug.Log($"Meowmentum reduced by {amount}. Chain: {currentChain}");
        }

        private void UpdateChainDecay()
        {
            if (currentChain == 0) return;

            timeSinceLastChain += Time.deltaTime;

            // Start decaying after inactivity
            if (timeSinceLastChain > chainDecayTime)
            {
                float decayRate = 0.5f; // Points per second
                float decay = decayRate * Time.deltaTime;

                currentChain -= (int)decay;
                currentChain = Mathf.Max(0, currentChain);

                MeowmentumLevel newLevel = CalculateLevel(currentChain);

                if (newLevel != currentLevel)
                {
                    currentLevel = newLevel;
                    UpdateEffects();
                    OnLevelChanged?.Invoke(currentLevel);
                }

                OnChainChanged?.Invoke(currentChain);
            }
        }

        private MeowmentumLevel CalculateLevel(int chain)
        {
            if (chain >= Constants.MEOWMENTUM_LEVEL_5) return MeowmentumLevel.Level5;
            if (chain >= Constants.MEOWMENTUM_LEVEL_4) return MeowmentumLevel.Level4;
            if (chain >= Constants.MEOWMENTUM_LEVEL_3) return MeowmentumLevel.Level3;
            if (chain >= Constants.MEOWMENTUM_LEVEL_2) return MeowmentumLevel.Level2;
            if (chain >= Constants.MEOWMENTUM_LEVEL_1) return MeowmentumLevel.Level1;
            return MeowmentumLevel.None;
        }

        private void LevelUp(MeowmentumLevel newLevel)
        {
            currentLevel = newLevel;
            UpdateEffects();
            OnLevelChanged?.Invoke(newLevel);

            // Play level up effects
            PlayLevelUpEffect();

            Debug.Log($"Meowmentum Level UP! {newLevel}");
        }

        private void LevelDown()
        {
            currentLevel = MeowmentumLevel.None;
            UpdateEffects();
            OnLevelChanged?.Invoke(MeowmentumLevel.None);

            Debug.Log("Meowmentum level reset");
        }

        private void UpdateEffects()
        {
            // Enable/disable visual effects based on level
            for (int i = 0; i < levelEffects.Length; i++)
            {
                if (levelEffects[i] != null)
                {
                    levelEffects[i].SetActive(i + 1 == (int)currentLevel);
                }
            }
        }

        private void PlayLevelUpEffect()
        {
            // TODO: Play particle effects, sound, screen effects
            // AudioManager.Instance.PlaySFX($"MeowmentumLevel{(int)currentLevel}");
        }

        /// <summary>
        /// Get current speed bonus multiplier
        /// </summary>
        public float GetSpeedBonus()
        {
            return speedBonuses[(int)currentLevel];
        }

        /// <summary>
        /// Get current stat bonus multiplier
        /// </summary>
        public float GetStatBonus()
        {
            return statBonuses[(int)currentLevel];
        }

        /// <summary>
        /// Get current chain count
        /// </summary>
        public int GetChainCount()
        {
            return currentChain;
        }

        /// <summary>
        /// Get current level
        /// </summary>
        public MeowmentumLevel GetCurrentLevel()
        {
            return currentLevel;
        }

        /// <summary>
        /// Check if at maximum level
        /// </summary>
        public bool IsAtMaxLevel()
        {
            return currentLevel == MeowmentumLevel.Level5;
        }

        /// <summary>
        /// Get progress to next level (0-1)
        /// </summary>
        public float GetProgressToNextLevel()
        {
            int currentThreshold = GetThresholdForLevel(currentLevel);
            int nextThreshold = GetThresholdForLevel(currentLevel + 1);

            if (nextThreshold == currentThreshold) return 1f;

            return (float)(currentChain - currentThreshold) / (nextThreshold - currentThreshold);
        }

        private int GetThresholdForLevel(MeowmentumLevel level)
        {
            return level switch
            {
                MeowmentumLevel.None => 0,
                MeowmentumLevel.Level1 => Constants.MEOWMENTUM_LEVEL_1,
                MeowmentumLevel.Level2 => Constants.MEOWMENTUM_LEVEL_2,
                MeowmentumLevel.Level3 => Constants.MEOWMENTUM_LEVEL_3,
                MeowmentumLevel.Level4 => Constants.MEOWMENTUM_LEVEL_4,
                MeowmentumLevel.Level5 => Constants.MEOWMENTUM_LEVEL_5,
                _ => 0
            };
        }

        /// <summary>
        /// Action scoring values
        /// </summary>
        public static class ChainPoints
        {
            public const int SUCCESSFUL_PASS = 1;
            public const int SUCCESSFUL_TACKLE = 1;
            public const int SHOT_ON_GOAL = 2;
            public const int DRIBBLE_PAST_OPPONENT = 2;
            public const int NUTMEG = 3;
            public const int GOAL = 5;
        }
    }
}
