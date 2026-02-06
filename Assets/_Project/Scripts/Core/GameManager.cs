using UnityEngine;
using CatFootball.Gameplay;

namespace CatFootball.Core
{
    /// <summary>
    /// Main game manager - initializes core systems and manages game state
    /// Persists across scenes
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Debug")]
        [SerializeField] private bool debugMode = true;

        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeGame();
        }

        private void InitializeGame()
        {
            if (debugMode) Debug.Log("GameManager: Initializing game systems...");

            // Set target frame rate
            Application.targetFrameRate = Constants.TARGET_FPS_GAMEPLAY;

            // Initialize core services
            InitializeServices();

            // Load player data
            LoadPlayerData();

            if (debugMode) Debug.Log("GameManager: Initialization complete");
        }

        private void InitializeServices()
        {
            // Register core services with ServiceLocator
            // These will be initialized as needed

            if (debugMode) Debug.Log("GameManager: Services registered");
        }

        private void LoadPlayerData()
        {
            // Check if first launch
            if (!PlayerPrefs.HasKey(Constants.PREF_FIRST_LAUNCH))
            {
                // First time playing
                PlayerPrefs.SetInt(Constants.PREF_FIRST_LAUNCH, 1);
                InitializeFirstTimePlayers();
            }

            // Load player settings
            LoadSettings();

            if (debugMode) Debug.Log("GameManager: Player data loaded");
        }

        private void InitializeFirstTimePlayers()
        {
            // Set default settings
            PlayerPrefs.SetFloat(Constants.PREF_MUSIC_VOLUME, 0.7f);
            PlayerPrefs.SetFloat(Constants.PREF_SFX_VOLUME, 0.8f);
            PlayerPrefs.SetInt(Constants.PREF_CONTROL_SCHEME, (int)ControlScheme.Classic);
            PlayerPrefs.Save();

            if (debugMode) Debug.Log("GameManager: First time player initialized");
        }

        private void LoadSettings()
        {
            // Load audio settings
            float musicVolume = PlayerPrefs.GetFloat(Constants.PREF_MUSIC_VOLUME, 0.7f);
            float sfxVolume = PlayerPrefs.GetFloat(Constants.PREF_SFX_VOLUME, 0.8f);

            // Apply settings to AudioManager when it exists
            // AudioManager will be implemented later

            if (debugMode) Debug.Log($"Settings loaded - Music: {musicVolume}, SFX: {sfxVolume}");
        }

        private void OnApplicationQuit()
        {
            // Save game state before quitting
            SaveGameState();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                // Save when app goes to background (important for mobile)
                SaveGameState();
            }
        }

        private void SaveGameState()
        {
            // Save player progress
            // This will be implemented with SaveLoadManager
            if (debugMode) Debug.Log("GameManager: Game state saved");
        }

        /// <summary>
        /// Quit the game (useful for UI buttons)
        /// </summary>
        public void QuitGame()
        {
            SaveGameState();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
