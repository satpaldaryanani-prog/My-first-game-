using UnityEngine;

namespace CatFootball
{
    /// <summary>
    /// Game-wide constants and configuration values
    /// </summary>
    public static class Constants
    {
        // Game Version
        public const string GAME_VERSION = "0.1.0";

        // Scene Names
        public const string SCENE_STARTUP = "Startup";
        public const string SCENE_MAIN_MENU = "MainMenu";
        public const string SCENE_TEAM_BUILDER = "TeamBuilder";
        public const string SCENE_STADIUM = "Stadium";
        public const string SCENE_COLLECTION = "Collection";
        public const string SCENE_LOADING = "Loading";

        // Layer Names
        public const string LAYER_PLAYER = "Player";
        public const string LAYER_BALL = "Ball";
        public const string LAYER_GROUND = "Ground";
        public const string LAYER_GOAL = "Goal";
        public const string LAYER_WALL = "Wall";

        // Tags
        public const string TAG_PLAYER = "Player";
        public const string TAG_BALL = "Ball";
        public const string TAG_GOAL = "Goal";

        // Gameplay Constants
        public const float MATCH_DURATION_QUICK = 180f; // 3 minutes
        public const float MATCH_DURATION_FULL = 360f; // 6 minutes
        public const int TEAM_SIZE = 3;
        public const int MAX_SUBSTITUTES = 3;

        // Ball Physics
        public const float BALL_MAX_SPEED = 20f;
        public const float BALL_FRICTION = 0.3f;
        public const float BALL_BOUNCINESS = 0.7f;

        // Character Stats Range
        public const float STAT_MIN = 1f;
        public const float STAT_MAX = 100f;

        // Stamina
        public const float STAMINA_MAX = 100f;
        public const float STAMINA_SPRINT_DRAIN_RATE = 20f; // per second
        public const float STAMINA_REGEN_RATE = 10f; // per second
        public const float STAMINA_CATNAP_THRESHOLD = 0f;
        public const float STAMINA_CATNAP_DURATION = 3f;
        public const float STAMINA_CATNAP_RESTORE = 20f;

        // Meowmentum System
        public const int MEOWMENTUM_LEVEL_1 = 3;
        public const int MEOWMENTUM_LEVEL_2 = 6;
        public const int MEOWMENTUM_LEVEL_3 = 10;
        public const int MEOWMENTUM_LEVEL_4 = 15;
        public const int MEOWMENTUM_LEVEL_5 = 20;

        // Currency
        public const string CURRENCY_FISH_COINS = "FishCoins";
        public const string CURRENCY_CATNIP_GEMS = "CatnipGems";

        // Gacha Drop Rates
        public const float GACHA_COMMON_RATE = 0.60f; // 60%
        public const float GACHA_RARE_RATE = 0.30f; // 30%
        public const float GACHA_EPIC_RATE = 0.09f; // 9%
        public const float GACHA_LEGENDARY_RATE = 0.01f; // 1%
        public const int GACHA_PITY_COUNT = 50;

        // Ability Cooldowns
        public const float COOLDOWN_POUNCE_STRIKE = 15f;
        public const float COOLDOWN_CATNIP_BOOST = 30f;
        public const float COOLDOWN_MEOW_TAUNT = 20f;
        public const float COOLDOWN_YARN_BALL_CURVE = 25f;
        public const float COOLDOWN_FURRY_FURY = 40f;
        public const float COOLDOWN_SHADOW_STEP = 18f;

        // Save System
        public const string SAVE_FILE_NAME = "catfootball_save.dat";
        public const string PREF_FIRST_LAUNCH = "FirstLaunch";
        public const string PREF_MUSIC_VOLUME = "MusicVolume";
        public const string PREF_SFX_VOLUME = "SFXVolume";
        public const string PREF_CONTROL_SCHEME = "ControlScheme";

        // UI
        public const float UI_TRANSITION_DURATION = 0.3f;
        public const float LOADING_MIN_DURATION = 1f;

        // Performance
        public const int TARGET_FPS_GAMEPLAY = 60;
        public const int TARGET_FPS_MENU = 30;
        public const int MAX_PARTICLES = 100;

        // Networking
        public const float NETWORK_TICK_RATE = 0.05f; // 20 Hz
        public const float NETWORK_TIMEOUT = 10f;
        public const int MAX_PLAYERS_PER_MATCH = 2;
    }
}
