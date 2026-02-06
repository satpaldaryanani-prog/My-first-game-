namespace CatFootball
{
    /// <summary>
    /// Character rarity levels
    /// </summary>
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    /// <summary>
    /// Character position types
    /// </summary>
    public enum CharacterPosition
    {
        Goalkeeper,
        Defender,
        Midfielder,
        Striker
    }

    /// <summary>
    /// Character evolution tiers
    /// </summary>
    public enum EvolutionTier
    {
        Kitten,
        Adult,
        Champion,
        Legend
    }

    /// <summary>
    /// Personality traits affecting AI behavior
    /// </summary>
    public enum PersonalityTrait
    {
        Playful,
        Lazy,
        Competitive,
        ScaredyCat,
        Balanced
    }

    /// <summary>
    /// Match outcome
    /// </summary>
    public enum MatchResult
    {
        Win,
        Loss,
        Draw
    }

    /// <summary>
    /// Game modes
    /// </summary>
    public enum GameMode
    {
        QuickMatch,
        Tournament,
        Career,
        Training,
        PvP,
        Ranked
    }

    /// <summary>
    /// Control scheme options
    /// </summary>
    public enum ControlScheme
    {
        Classic,        // Joystick + Buttons
        Gesture,        // Swipe controls
        Hybrid          // Combination
    }

    /// <summary>
    /// Team formations
    /// </summary>
    public enum Formation
    {
        Formation_1_2_2,    // Defensive
        Formation_1_1_3,    // Attacking
        Formation_1_3_1,    // Balanced
        Formation_2_2_1,    // Control
        CardboardBox,       // Special: Cardboard Box Defense
        CatTower,           // Special: Cat Tower Attack
        RedDotChase,        // Special: Red Dot Chase
        Loaf                // Special: Loaf Formation
    }

    /// <summary>
    /// Weather conditions
    /// </summary>
    public enum Weather
    {
        Sunny,
        Rainy,
        Windy,
        Night,
        Snow
    }

    /// <summary>
    /// Meowmentum levels
    /// </summary>
    public enum MeowmentumLevel
    {
        None = 0,
        Level1 = 1,     // Mild Interest
        Level2 = 2,     // Growing Excitement
        Level3 = 3,     // High Energy
        Level4 = 4,     // Purrfect Flow
        Level5 = 5      // PURRFECT PLAY
    }

    /// <summary>
    /// Curiosity distraction types
    /// </summary>
    public enum DistractionType
    {
        BirdFlying,
        LaserPointer,
        CardboardBox,
        SunbeamNap,
        MysteriousNoise
    }

    /// <summary>
    /// Ability types
    /// </summary>
    public enum AbilityType
    {
        PounceStrike,
        CatnipBoost,
        MeowTaunt,
        YarnBallCurve,
        FurryFury,
        ShadowStep,
        CatBurglar,
        CatastropheKick,
        RoyalDecree,
        NineLivesSave
    }

    /// <summary>
    /// Character animation states
    /// </summary>
    public enum AnimationState
    {
        Idle,
        Run,
        Sprint,
        Kick,
        Pass,
        Tackle,
        Celebrate,
        Disappointed,
        Hit,
        SpecialAbility,
        Catnap
    }

    /// <summary>
    /// AI difficulty levels
    /// </summary>
    public enum AIDifficulty
    {
        Easy,
        Medium,
        Hard,
        Expert
    }

    /// <summary>
    /// Match events
    /// </summary>
    public enum MatchEvent
    {
        MatchStart,
        Goal,
        HalfTime,
        MatchEnd,
        Foul,
        FreeKick,
        CornerKick,
        PenaltyKick,
        Substitution
    }

    /// <summary>
    /// Team side
    /// </summary>
    public enum TeamSide
    {
        Home,
        Away
    }

    /// <summary>
    /// Input actions
    /// </summary>
    public enum InputAction
    {
        Move,
        Pass,
        Shoot,
        Sprint,
        Tackle,
        SwitchPlayer,
        SpecialAbility
    }
}
