# Project Structure

## Recommended Unity Project Organization

```
CatFootball/                              # Unity Project Root
│
├── Assets/                               # Unity Assets Folder
│   │
│   ├── _Project/                         # Main project assets (easier to identify)
│   │   │
│   │   ├── Scripts/                      # All C# scripts
│   │   │   ├── Core/                     # Core game systems
│   │   │   │   ├── GameManager.cs
│   │   │   │   ├── SceneLoader.cs
│   │   │   │   ├── ServiceLocator.cs
│   │   │   │   └── SaveLoadManager.cs
│   │   │   │
│   │   │   ├── Characters/               # Character-related scripts
│   │   │   │   ├── CatCharacter.cs
│   │   │   │   ├── CatController.cs
│   │   │   │   ├── CatAnimator.cs
│   │   │   │   ├── CatStats.cs
│   │   │   │   ├── CatAbilities.cs
│   │   │   │   └── CatAI.cs
│   │   │   │
│   │   │   ├── Gameplay/                 # Gameplay mechanics
│   │   │   │   ├── Match/
│   │   │   │   │   ├── MatchManager.cs
│   │   │   │   │   ├── MatchRules.cs
│   │   │   │   │   ├── ScoreTracker.cs
│   │   │   │   │   └── MatchTimer.cs
│   │   │   │   │
│   │   │   │   ├── Ball/
│   │   │   │   │   ├── BallPhysics.cs
│   │   │   │   │   ├── BallController.cs
│   │   │   │   │   └── TrajectoryPredictor.cs
│   │   │   │   │
│   │   │   │   ├── Input/
│   │   │   │   │   ├── InputManager.cs
│   │   │   │   │   ├── VirtualJoystick.cs
│   │   │   │   │   ├── GestureDetector.cs
│   │   │   │   │   └── ControlSchemeManager.cs
│   │   │   │   │
│   │   │   │   ├── AI/
│   │   │   │   │   ├── AIController.cs
│   │   │   │   │   ├── AIBehaviorTree.cs
│   │   │   │   │   ├── GoalkeeperAI.cs
│   │   │   │   │   └── DifficultyManager.cs
│   │   │   │   │
│   │   │   │   ├── UniqueSystem/         # Cat-themed unique mechanics
│   │   │   │   │   ├── CuriositySystem.cs
│   │   │   │   │   ├── MeowmentumChain.cs
│   │   │   │   │   ├── TerritoryMarking.cs
│   │   │   │   │   ├── WeatherSystem.cs
│   │   │   │   │   └── FormationManager.cs
│   │   │   │   │
│   │   │   │   └── Abilities/
│   │   │   │       ├── AbilityBase.cs
│   │   │   │       ├── PounceStrike.cs
│   │   │   │       ├── CatnipBoost.cs
│   │   │   │       ├── YarnBallCurve.cs
│   │   │   │       └── FurryFury.cs
│   │   │   │
│   │   │   ├── UI/                       # UI Scripts
│   │   │   │   ├── MainMenu/
│   │   │   │   │   ├── MainMenuController.cs
│   │   │   │   │   ├── SettingsMenu.cs
│   │   │   │   │   └── ModeSelector.cs
│   │   │   │   │
│   │   │   │   ├── Match/
│   │   │   │   │   ├── MatchHUD.cs
│   │   │   │   │   ├── PauseMenu.cs
│   │   │   │   │   ├── StaminaBar.cs
│   │   │   │   │   └── AbilityCooldownUI.cs
│   │   │   │   │
│   │   │   │   ├── Collection/
│   │   │   │   │   ├── CharacterCard.cs
│   │   │   │   │   ├── CollectionScreen.cs
│   │   │   │   │   ├── TeamBuilder.cs
│   │   │   │   │   └── CharacterDetailView.cs
│   │   │   │   │
│   │   │   │   ├── Gacha/
│   │   │   │   │   ├── PackOpeningUI.cs
│   │   │   │   │   ├── ShopUI.cs
│   │   │   │   │   └── PackAnimator.cs
│   │   │   │   │
│   │   │   │   └── Common/
│   │   │   │       ├── UITransitions.cs
│   │   │   │       ├── ButtonSounds.cs
│   │   │   │       └── LoadingScreen.cs
│   │   │   │
│   │   │   ├── Data/                     # Data containers and ScriptableObjects
│   │   │   │   ├── CatCharacterData.cs
│   │   │   │   ├── StadiumData.cs
│   │   │   │   ├── AbilityData.cs
│   │   │   │   ├── MatchRulesConfig.cs
│   │   │   │   └── BalanceConfig.cs
│   │   │   │
│   │   │   ├── Progression/              # Leveling, evolution, economy
│   │   │   │   ├── ExperienceSystem.cs
│   │   │   │   ├── LevelingManager.cs
│   │   │   │   ├── EvolutionManager.cs
│   │   │   │   ├── CurrencyManager.cs
│   │   │   │   └── RewardCalculator.cs
│   │   │   │
│   │   │   ├── Network/                  # Multiplayer networking
│   │   │   │   ├── NetworkManager.cs
│   │   │   │   ├── MatchmakingService.cs
│   │   │   │   ├── NetworkPlayer.cs
│   │   │   │   ├── ServerAuthority.cs
│   │   │   │   └── LagCompensation.cs
│   │   │   │
│   │   │   ├── Audio/                    # Audio management
│   │   │   │   ├── AudioManager.cs
│   │   │   │   ├── MusicController.cs
│   │   │   │   └── SFXPlayer.cs
│   │   │   │
│   │   │   ├── Events/                   # ScriptableObject events (decoupling)
│   │   │   │   ├── GameEvent.cs
│   │   │   │   ├── GameEventListener.cs
│   │   │   │   └── Events/               # Individual event instances
│   │   │   │
│   │   │   └── Utilities/                # Helper scripts
│   │   │       ├── ObjectPool.cs
│   │   │       ├── Singleton.cs
│   │   │       ├── ExtensionMethods.cs
│   │   │       └── Constants.cs
│   │   │
│   │   ├── Art/                          # All visual assets
│   │   │   ├── Sprites/
│   │   │   │   ├── Characters/           # Cat character sprites
│   │   │   │   │   ├── Common/
│   │   │   │   │   ├── Rare/
│   │   │   │   │   ├── Epic/
│   │   │   │   │   └── Legendary/
│   │   │   │   │
│   │   │   │   ├── UI/                   # UI elements
│   │   │   │   │   ├── Buttons/
│   │   │   │   │   ├── Icons/
│   │   │   │   │   ├── Backgrounds/
│   │   │   │   │   └── Cards/
│   │   │   │   │
│   │   │   │   ├── Environment/          # Stadiums, backgrounds
│   │   │   │   │   ├── Stadiums/
│   │   │   │   │   ├── Crowds/
│   │   │   │   │   └── Props/
│   │   │   │   │
│   │   │   │   ├── Effects/              # Visual effects sprites
│   │   │   │   │   ├── Particles/
│   │   │   │   │   ├── Impacts/
│   │   │   │   │   └── Celebrations/
│   │   │   │   │
│   │   │   │   └── Ball/                 # Ball variants
│   │   │   │       ├── Standard.png
│   │   │   │       ├── YarnBall.png
│   │   │   │       └── GlowBall.png
│   │   │   │
│   │   │   ├── Animations/               # Animation files
│   │   │   │   ├── Cats/
│   │   │   │   │   ├── Idle.anim
│   │   │   │   │   ├── Run.anim
│   │   │   │   │   ├── Kick.anim
│   │   │   │   │   ├── Celebrate.anim
│   │   │   │   │   └── Sad.anim
│   │   │   │   │
│   │   │   │   └── UI/
│   │   │   │       └── Transitions.anim
│   │   │   │
│   │   │   ├── Materials/                # Materials for 3D (if used)
│   │   │   │   └── ToonShader.mat
│   │   │   │
│   │   │   └── Shaders/                  # Custom shaders
│   │   │       └── CelShaded.shader
│   │   │
│   │   ├── Prefabs/                      # Prefabricated GameObjects
│   │   │   ├── Characters/
│   │   │   │   ├── Cats/                 # Cat prefabs
│   │   │   │   │   ├── Whiskers.prefab
│   │   │   │   │   ├── Zoom.prefab
│   │   │   │   │   └── Tank.prefab
│   │   │   │   │
│   │   │   │   └── Ball.prefab
│   │   │   │
│   │   │   ├── UI/                       # UI prefabs
│   │   │   │   ├── CharacterCard.prefab
│   │   │   │   ├── AbilityButton.prefab
│   │   │   │   └── PackOpening.prefab
│   │   │   │
│   │   │   ├── Environment/
│   │   │   │   ├── Stadium_Basic.prefab
│   │   │   │   └── Crowd_Section.prefab
│   │   │   │
│   │   │   └── Effects/
│   │   │       ├── KickEffect.prefab
│   │   │       ├── GoalCelebration.prefab
│   │   │       └── PawTrail.prefab
│   │   │
│   │   ├── Scenes/                       # Unity scenes
│   │   │   ├── Startup.unity             # Initial loading scene
│   │   │   ├── MainMenu.unity            # Main menu
│   │   │   ├── TeamBuilder.unity         # Team management
│   │   │   ├── Stadium.unity             # Main gameplay scene
│   │   │   ├── Collection.unity          # Character collection
│   │   │   └── Loading.unity             # Loading screen
│   │   │
│   │   ├── Audio/                        # All audio files
│   │   │   ├── Music/
│   │   │   │   ├── MainMenu.mp3
│   │   │   │   ├── Match_Dynamic.mp3
│   │   │   │   ├── Victory.mp3
│   │   │   │   └── Defeat.mp3
│   │   │   │
│   │   │   ├── SFX/
│   │   │   │   ├── Kicks/
│   │   │   │   │   ├── Kick_Light.wav
│   │   │   │   │   ├── Kick_Medium.wav
│   │   │   │   │   └── Kick_Heavy.wav
│   │   │   │   │
│   │   │   │   ├── Meows/
│   │   │   │   │   ├── Meow_Happy.wav
│   │   │   │   │   ├── Meow_Sad.wav
│   │   │   │   │   └── Meow_Angry.wav
│   │   │   │   │
│   │   │   │   ├── UI/
│   │   │   │   │   ├── Button_Click.wav
│   │   │   │   │   ├── Transition.wav
│   │   │   │   │   └── CardFlip.wav
│   │   │   │   │
│   │   │   │   ├── Crowd/
│   │   │   │   │   ├── Cheer.wav
│   │   │   │   │   ├── Disappointed.wav
│   │   │   │   │   └── Meowing_Crowd.wav
│   │   │   │   │
│   │   │   │   └── Match/
│   │   │   │       ├── Whistle.wav
│   │   │   │       ├── Goal_Horn.wav
│   │   │   │       └── Ball_Bounce.wav
│   │   │   │
│   │   │   └── Voices/
│   │   │       └── Coach_Cat/            # Tutorial voice lines
│   │   │
│   │   ├── Data/                         # ScriptableObject instances
│   │   │   ├── Characters/               # Character data assets
│   │   │   │   ├── Whiskers_Data.asset
│   │   │   │   ├── Zoom_Data.asset
│   │   │   │   └── Tank_Data.asset
│   │   │   │
│   │   │   ├── Stadiums/
│   │   │   │   ├── BasicField.asset
│   │   │   │   └── YarnArena.asset
│   │   │   │
│   │   │   ├── Abilities/
│   │   │   │   ├── PounceStrike_Data.asset
│   │   │   │   └── CatnipBoost_Data.asset
│   │   │   │
│   │   │   ├── Events/                   # Event instances
│   │   │   │   ├── OnMatchStart.asset
│   │   │   │   ├── OnGoalScored.asset
│   │   │   │   └── OnPlayerInput.asset
│   │   │   │
│   │   │   └── Config/
│   │   │       ├── GameBalance.asset     # Tweakable values
│   │   │       └── MatchRules.asset
│   │   │
│   │   ├── Resources/                    # Unity Resources folder (use sparingly)
│   │   │   └── DefaultConfig.asset
│   │   │
│   │   └── StreamingAssets/              # Assets loaded at runtime
│   │       └── Addressables/             # Addressable assets (downloaded content)
│   │
│   ├── Packages/                         # Unity Package Manager packages
│   │   └── manifest.json
│   │
│   ├── ProjectSettings/                  # Unity project settings
│   │   ├── ProjectSettings.asset
│   │   ├── InputManager.asset
│   │   └── ...
│   │
│   └── UserSettings/                     # User-specific settings (gitignored)
│
├── Documentation/                        # Project documentation (outside Unity)
│   ├── GAME_DESIGN.md
│   ├── TECHNICAL_STACK.md
│   ├── CHARACTER_DESIGN.md
│   ├── ENHANCED_GAMEPLAY.md
│   ├── DEVELOPMENT_ROADMAP.md
│   ├── PROJECT_STRUCTURE.md
│   └── ArtStyleGuide/                    # Art reference images
│
├── Backend/                              # Backend code (separate from Unity)
│   ├── node-server/                      # Node.js backend
│   │   ├── src/
│   │   ├── package.json
│   │   └── ...
│   │
│   └── firebase/                         # Firebase config
│       └── functions/
│
├── Tools/                                # Development tools and scripts
│   ├── build-scripts/
│   └── asset-tools/
│
├── .gitignore
├── .gitattributes                        # For Git LFS
└── README.md

```

## Key Organizational Principles

### 1. **_Project Folder Convention**
All custom project assets go in `Assets/_Project/` to clearly separate them from imported packages and Unity defaults.

### 2. **Script Organization**
Scripts are organized by **function** (Core, Characters, Gameplay, UI) not by type (all scripts in one folder).

### 3. **Data-Driven Design**
Use ScriptableObjects extensively:
- Character stats
- Game balance values
- Event systems
- Configuration

This allows designers to modify game values without touching code.

### 4. **Prefab Organization**
Prefabs mirror the script organization (Characters, UI, Environment, Effects).

### 5. **Addressables for Scale**
Use Addressables system for:
- Character assets (load on demand)
- Stadiums
- Audio files
- Special effects

This keeps the initial download small and allows for remote content updates.

### 6. **Separation of Concerns**
- **Assets/** - Unity project
- **Documentation/** - Design docs, guides
- **Backend/** - Server code
- **Tools/** - Build scripts, automation

## Asset Naming Conventions

### Scripts
- PascalCase: `CatController.cs`, `MatchManager.cs`
- Interfaces: `IControllable.cs`
- Abstract: `AbilityBase.cs`

### Prefabs
- PascalCase with type suffix: `Whiskers_Cat.prefab`, `Stadium_Basic.prefab`

### Sprites
- lowercase_snake_case: `cat_idle_01.png`, `button_play.png`

### Audio
- PascalCase with category: `Kick_Heavy.wav`, `Meow_Happy.wav`

### ScriptableObjects
- PascalCase with suffix: `Whiskers_Data.asset`, `GameBalance_Config.asset`

## Version Control Best Practices

### Use Git LFS for:
- All images (*.png, *.jpg, *.psd)
- Audio files (*.wav, *.mp3)
- 3D models (*.fbx, *.obj)
- Unity assets (*.prefab, *.unity, *.asset)

### Example .gitattributes:
```
*.png filter=lfs diff=lfs merge=lfs -text
*.jpg filter=lfs diff=lfs merge=lfs -text
*.psd filter=lfs diff=lfs merge=lfs -text
*.wav filter=lfs diff=lfs merge=lfs -text
*.mp3 filter=lfs diff=lfs merge=lfs -text
*.fbx filter=lfs diff=lfs merge=lfs -text
*.prefab filter=lfs diff=lfs merge=lfs -text
*.unity filter=lfs diff=lfs merge=lfs -text
```

## Important Notes

1. **Never commit /Library/** - Always in .gitignore
2. **Always commit ProjectSettings/** - Contains essential configuration
3. **Gitignore UserSettings/** - User-specific preferences
4. **Use Scenes in Addressables** - For easier content updates
5. **Keep Resources/ minimal** - Prefer Addressables for runtime loading

---

**Version**: 1.0
**Last Updated**: 2026-02-06
**Status**: Planning Phase
