# Cat Football - Playable Browser Demo 🐱⚽

## How to Play

### Option 1: Direct File Open
1. Open `index.html` in your web browser
2. That's it! Start playing immediately

### Option 2: Local Server (Recommended)
```bash
cd demo
python3 -m http.server 8080
```
Then open: http://localhost:8080

---

## 🎮 Controls

| Key | Action |
|-----|--------|
| **W A S D** | Move your cat (Whiskers) |
| **Shift** | Sprint (drains stamina) |
| **Space** | Shoot/Pass ball |
| **E** | Power kick (stronger shot) |
| **R** | Restart match |

---

## 🎯 Objective

Score more goals than the AI opponent (Shadow) before time runs out!

- **Match Duration:** 3 minutes
- **Your Goal:** Left side (defend it!)
- **Opponent Goal:** Right side (score there!)

---

## ⚡ Meowmentum System

Build your chain by:
- **Ball control:** +1 point
- **Shot on goal:** +2 points
- **Power shot:** +2 points
- **Goal:** +5 points

### Levels & Bonuses:
- **Level 1** (3 chains): +5% speed boost
- **Level 2** (6 chains): +10% speed boost
- **Level 3** (10 chains): +15% speed boost 🔥
- **Level 4** (15 chains): +20% speed boost 🔥🔥
- **Level 5** (20 chains): +25% speed boost 🔥🔥🔥

**Warning:** Chain breaks if opponent takes the ball!

---

## 💪 Stamina System

- **Sprint drains stamina** at 15% per second
- **Regenerates** at 10% per second when not sprinting
- **Can't sprint** when stamina is depleted
- **Watch the green bar** under your cat!

---

## 🎨 Visual Features

### Your Cat (Whiskers)
- **Orange color** 🧡
- **Left side** of field
- **Stamina bar** displayed

### AI Cat (Shadow)
- **Purple color** 💜
- **Right side** of field
- Chases the ball automatically

### Ball
- **White with black spots**
- **Speed trails** when moving fast
- **Bounces off walls**

### Meowmentum Effects
As you level up, visual effects appear:
- Glowing aura around your cat
- Paw print particles
- Color intensity increases with level

---

## 📊 UI Information

### Match Info Panel
- **Score:** Your goals vs AI goals
- **Time:** Countdown timer
- **Possession:** Who has the ball

### Meowmentum Panel
- **Progress bar:** Visual chain progress
- **Chain count:** Current chain points
- **Bonus:** Current speed bonus percentage

### Player Stats Panel
- **Stamina:** Current energy level
- **Speed:** Current movement speed (with bonuses)
- **Status:** Current condition (Normal, Tired, On Fire!)

---

## 💡 Tips & Strategy

1. **Manage your stamina** - Don't sprint constantly!
2. **Build Meowmentum chains** - Speed bonus helps a lot
3. **Position yourself** - Block AI from getting to the ball
4. **Time your shots** - Aim for the goal opening
5. **Use power kicks (E)** - When you have a clear shot
6. **Keep the ball** - Each action builds your chain
7. **Sprint strategically** - When you need to beat AI to the ball

---

## 🎮 Gameplay Features Demonstrated

This demo showcases all core mechanics from the full game:

✅ **Ball Physics**
- Realistic movement with friction
- Bouncing off walls
- Speed limitations
- Possession tracking

✅ **Character Control**
- Smooth WASD movement
- Sprint with stamina drain
- Ball interactions
- Goal-directed shooting

✅ **Match Management**
- 3-minute timer
- Score tracking
- Goal detection
- Match end conditions

✅ **Meowmentum System**
- 5-level progression
- Chain building mechanics
- Progressive bonuses
- Visual feedback

✅ **Stamina System**
- Sprint drain
- Passive regeneration
- Prevents infinite sprinting
- Visual indicator

✅ **AI Opponent**
- Ball chasing behavior
- Automatic shooting
- Breaks your Meowmentum chains

---

## 🐛 Known Limitations

This is a **proof-of-concept demo**, not the full game:

- **Simple AI** - Just chases ball, no advanced tactics
- **No advanced abilities** - Core mechanics only
- **No animations** - Static sprites
- **No sound** - Visual only
- **No formations** - Just 1v1 gameplay
- **No evolution/progression** - Match-only demo

The **full Unity game** will include:
- 3v3 matches
- Advanced AI with personalities
- Special abilities (Pounce Strike, Catnip Boost, etc.)
- Character animations
- Sound effects and music
- Multiple game modes
- Character collection and evolution
- Curiosity System (distractions)
- Weather effects
- And much more!

---

## 🚀 Performance

This demo runs at **60 FPS** in modern browsers and demonstrates that the core gameplay loop is **fun and responsive**!

---

## 📝 Technical Implementation

Built with:
- **HTML5 Canvas** for rendering
- **Vanilla JavaScript** for game logic
- **Requestation Frame** for smooth 60 FPS
- **Real-time physics** calculations
- **Event-driven** input handling

All mechanics match the **Unity C# implementation** in the main codebase!

---

## 🎉 Have Fun!

This demo proves the core concept works and feels great! Enjoy playing, and imagine all the additional features coming in the full Unity version!

**Try to:**
- Score 5 goals before time runs out
- Reach Meowmentum Level 5
- Win without letting AI score

---

**Created:** 2026-02-06
**Status:** Playable Demo v1.0
**Next:** Full Unity implementation with 3v3, abilities, and progression!
