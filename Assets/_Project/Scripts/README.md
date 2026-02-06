# Cat Football - Scripts Documentation

## Overview

This directory contains all C# scripts for the Cat Football game, organized by functionality following clean architecture principles.

## Directory Structure

```
Scripts/
├── Core/                     # Core game systems
├── Characters/               # Character controllers and behavior
├── Gameplay/                 # Gameplay mechanics
│   ├── Match/               # Match management
│   ├── Ball/                # Ball physics
│   ├── Input/               # Input handling
│   ├── AI/                  # AI controllers
│   ├── UniqueSystem/        # Cat-themed unique mechanics
│   └── Abilities/           # Special abilities
├── UI/                      # User interface
├── Data/                    # ScriptableObject definitions
├── Progression/             # Leveling and progression
├── Network/                 # Multiplayer networking
├── Audio/                   # Audio management
├── Events/                  # Event system
└── Utilities/               # Helper scripts
```

## Core Systems

### GameManager.cs
Main entry point and game lifecycle manager.
- Initializes core systems
- Manages game state
- Handles save/load on app lifecycle events
- Singleton pattern with DontDestroyOnLoad

### ServiceLocator.cs
Dependency injection pattern for managing game services.
- Register services at startup
- Access services from anywhere without singletons
- Loose coupling between systems

**Usage:**
```csharp
// Register a service
ServiceLocator.Instance.Register<MatchManager>(matchManager);

// Get a service
var matchManager = ServiceLocator.Instance.Get<MatchManager>();
```

## Data-Driven Design

### ScriptableObjects

All game data uses ScriptableObjects for designer-friendly workflows:

#### CatCharacterData.cs
Defines a cat character's stats and properties.
- Stats: Pounce Power, Whisker Precision, Tail Balance, etc.
- Personality traits
- Evolution data
- Abilities list

**Creating Characters:**
1. Right-click in Project → Create → Cat Football → Character Data
2. Fill in stats and properties
3. Reference in prefabs or scripts

#### AbilityData.cs
Defines special abilities.
- Cooldown times
- Effect values
- Visual/audio effects
- Requirements

## Gameplay Systems

### Ball Physics (BallController.cs)
Handles ball movement and interactions.
- Realistic physics with tunable parameters
- Kick, pass, and spin methods
- Possession tracking
- Collision response

**Key Methods:**
```csharp
ball.Kick(direction, power, kicker);
ball.Pass(targetPosition, accuracy, passer);
ball.ApplySpin(spinAmount);
```

### Character Control (CatController.cs)
Main controller for cat characters.
- Movement and sprinting
- Ball possession
- Stamina management
- Catnap mechanic (when exhausted)
- Animation state management

**Stat-Based Behavior:**
- Movement speed based on Tail Balance stat
- Kick power based on Pounce Power stat
- Pass accuracy based on Whisker Precision stat

### Match Management (MatchManager.cs)
Orchestrates match flow and rules.
- Match timer
- Score tracking
- Goal registration
- Pause/resume
- Kickoff resets

**Events:**
- OnMatchStart
- OnGoalScored
- OnMatchEnd
- OnTimeUpdate

### Input System (InputManager.cs)
Handles player input across multiple control schemes.

**Supported Schemes:**
- Classic: Joystick + buttons (EA Mobile style)
- Gesture: Touch and swipe controls
- Hybrid: Combination of both

**Actions:**
- Movement
- Pass, Shoot, Tackle
- Sprint
- Switch player

### Meowmentum System (MeowmentumManager.cs)
Unique progression system that rewards skillful play.

**Levels:**
- Level 1 (3+ chains): +5% speed
- Level 2 (6+ chains): +10% speed, +5% accuracy
- Level 3 (10+ chains): +15% all stats
- Level 4 (15+ chains): +20% all stats, ability cooldown -10%
- Level 5 (20+ chains): +25% all stats, +90% shot accuracy

**Chain Points:**
- Pass: +1
- Tackle: +1
- Shot on goal: +2
- Dribble past opponent: +2
- Nutmeg: +3
- Goal: +5

### Ability System (AbilitySystem.cs)
Manages character abilities and cooldowns.
- Ability execution
- Cooldown tracking
- Visual and audio feedback

**Key Abilities:**
- Pounce Strike: Quick dash to ball
- Catnip Boost: Speed boost
- Yarn Ball Curve: Curved shot
- Furry Fury: Power shot

## Constants and Enums

### Constants.cs
All game-wide constant values.
- Scene names
- Layer names and tags
- Gameplay values (match duration, team size, etc.)
- Physics parameters
- Currency names
- Gacha drop rates
- Performance targets

### Enums.cs
All enumerations used throughout the game.
- Rarity, CharacterPosition, EvolutionTier
- GameMode, MatchResult
- ControlScheme, Formation
- Weather, MeowmentumLevel
- AbilityType, AnimationState

## Coding Standards

### Naming Conventions
- Classes: PascalCase (GameManager, BallController)
- Methods: PascalCase (Kick, SetPosition)
- Private fields: camelCase with underscore (\_rb, \_currentState)
- SerializedFields: camelCase (ballSpeed, maxPower)
- Constants: UPPER_SNAKE_CASE (MATCH_DURATION, TEAM_SIZE)

### Documentation
- XML documentation for public methods
- Inline comments for complex logic
- Header comments for each class

### Unity Best Practices
- Use SerializeField for inspector-visible private fields
- Cache component references in Awake()
- Use RequireComponent for dependencies
- Implement OnDrawGizmos for debug visualization
- Use Events (UnityEvent) for decoupling

## Performance Considerations

### Object Pooling
Implement pooling for:
- Particle effects
- UI elements (score popups)
- Audio sources

### Update Optimization
- Minimize operations in Update/FixedUpdate
- Cache frequently accessed components
- Use events instead of polling

### Mobile Optimization
- Target 60 FPS on mid-range devices
- Keep physics simple (2D, limited bodies)
- Minimize draw calls (batching)
- Use Addressables for on-demand loading

## Testing

### Play Mode Tests
Use Unity Test Framework for:
- Ball physics accuracy
- Character stat calculations
- Ability cooldown logic
- Match timer accuracy

### Editor Testing
- Use Debug.Log for development
- Enable debug gizmos for visualization
- Test with keyboard in editor, touch on device

## Next Steps

### Immediate Priorities:
1. ✅ Core systems (GameManager, ServiceLocator)
2. ✅ Data structures (CatCharacterData, AbilityData)
3. ✅ Ball physics
4. ✅ Character controller
5. ✅ Match management
6. ✅ Input system
7. ✅ Meowmentum system
8. ✅ Ability system

### To Be Implemented:
- [ ] AI controllers (AIController.cs)
- [ ] Curiosity System (distractions)
- [ ] UI scripts (HUD, menus, cards)
- [ ] Save/Load system
- [ ] Audio manager
- [ ] Progression system (leveling, evolution)
- [ ] Gacha system
- [ ] Networking (multiplayer)

## Integration Guide

### Setting Up a New Scene

1. **Add GameManager:**
   ```
   Create Empty GameObject → Add GameManager.cs
   Mark as DontDestroyOnLoad
   ```

2. **Set Up Match:**
   ```
   Create Empty GameObject → Add MatchManager.cs
   Assign ball, goals, teams
   ```

3. **Add Input:**
   ```
   Create Empty GameObject → Add InputManager.cs
   Reference controlled character
   ```

### Creating a Cat Character

1. **Create Data Asset:**
   - Right-click → Create → Cat Football → Character Data
   - Fill in stats, personality, abilities

2. **Create Prefab:**
   - GameObject with Rigidbody2D, CircleCollider2D
   - Add CatController.cs
   - Add Animator component
   - Add AbilitySystem.cs
   - Assign character data

3. **Set Up Animation:**
   - Create Animator Controller
   - Add animation states (Idle, Run, Sprint, Kick, etc.)
   - Set parameters: AnimState (int), Speed (float), HasBall (bool)

### Adding a New Ability

1. **Create Data Asset:**
   - Right-click → Create → Cat Football → Ability Data
   - Set cooldown, effects, requirements

2. **Implement Logic:**
   - Add case in AbilitySystem.ExecuteAbility()
   - Create specific execution method
   - Handle visual/audio feedback

3. **Assign to Character:**
   - Add ability to character's AbilityData list
   - Configure in inspector

## Support

For questions or issues, refer to:
- Main documentation: `/Documentation/`
- Game design: `GAME_DESIGN.md`
- Technical architecture: `TECHNICAL_STACK.md`

---

**Version:** 0.1.0
**Last Updated:** 2026-02-06
**Status:** Initial Development
