# 🧟 Bitten — Unity 3D PC Game

Bitten is a 3D zombie action game built with **Unity and C#**. Fight through enemies, manage your remaining time, switch weapons, and reach the serum to complete each level.

Developed over approximately **nine months**, Bitten is a learning and portfolio project that brings character control, enemy AI, combat, animation, UI, audio, and level progression together in a playable PC game.

---

## 🧠 Project Story — What I Practiced & Learned

Bitten grew out of roughly a year of learning Unity through a Udemy course, YouTube tutorials, other resources, and hands-on experimentation. After working on my earlier [Brick Breaker project](https://github.com/erenyusufcan/BrickBreaker), I continued with the course's 3D PC game section.

At first, I followed the lessons step by step while trying to understand the reasoning behind the code. As I became more comfortable, I began reading a lesson's title or watching its introduction, then attempting the feature myself before returning to the explanation.

My focus gradually expanded from making individual mechanics work to understanding how they fit together: which scripts need access to the GameDirector, how managers coordinate gameplay, and where a spawned prefab should receive its references and begin its behavior.

Throughout development, I also experimented with gameplay details, adjusted timings and feedback, and worked through problems using additional resources. This repository represents that learning process and the game I completed through it.

### What I focused on

- Defining responsibilities across gameplay scripts, managers, and UI components.
- Passing references into spawned objects through initialization methods.
- Coordinating gameplay with explicit game states.
- Connecting physics, animation, combat, and player feedback.
- Using coroutines for timed actions and object pooling for reusable projectiles.
- Iterating on movement, weapon behavior, and audiovisual feedback.

---

## 🎮 Gameplay Overview

- Move and aim independently with keyboard and mouse.
- Fight melee and ranged zombies using a machine gun, shotgun, and grenades.
- Sprint using stamina or dash to reposition.
- Collect enemy drops to recover time.
- Clear enemies to open the exit door, then reach the serum to win.
- Avoid losing all health, falling out of the level, or running out of time.
- Continue through repeating level layouts with additional enemies on later cycles.

---

## ⚙️ Features & Systems

### 🏃 Character Movement

Rigidbody-based movement, mouse-directed aiming, ground checks, directional animation, stamina consumption and recovery, and a dash with a cooldown. The movement script also supports a configurable jump mode; the main scene uses dash.

Weapon choice affects movement: the main scene configures a walking speed of **5 with the machine gun** and **3 with the shotgun**. Sprinting also uses a reduced speed when carrying the shotgun.

### 🔫 Combat & Weapons

- Machine gun with ammunition tracking, automatic reload, and an ammunition display.
- Different machine-gun firing intervals while stationary and moving: **0.065 seconds** and **0.1 seconds** in the weapon prefab.
- Shotgun with a randomized multi-projectile spread.
- Weapon switching through keyboard shortcuts and the inventory interface.
- Bullet pools that reuse projectiles after collisions or travel-range limits.
- Mouse-targeted grenade throws with an animated arc, cooldown display, and distance-based explosion damage.

These are examples of the systems and tuning present in this version. The project combines course-guided work with my own implementation attempts, adjustments, and experimentation.

### 🧟 Enemy Behavior

Enemies use **NavMeshAgent** navigation and raycasts to detect obstacles between themselves and the player. Their behavior includes standing, chasing, moving toward the player's last seen position, attacking, and dying.

Melee attacks use timed hit checks; ranged enemies launch projectiles toward the player's position at the time of firing. Health bars, hit reactions, animation transitions, damage text, and death effects communicate combat results.

### ⏱️ Levels & Game Flow

The GameDirector coordinates starting, restarting, winning, and losing a level. LevelManager creates level prefabs and cycles through the available layouts. Later cycles add enemies through spawners and adjust the starting timer.

Enemy drops restore time, doors respond to level completion, and environmental traps add hazards. An introductory video is integrated into the start flow.

### ✨ UI, Audio & Feedback

- Main menu, escape menu, and win/lose screens.
- Timer, health, stamina, ammunition, grenade cooldown, and level displays.
- Inventory buttons and contextual messages.
- DOTween transitions, muzzle flashes, impact particles, hit flashes, damage text, and camera shake.
- Audio management for weapons, enemies, impacts, movement, and game outcomes.

---

## 🔍 Code Highlights

These files are useful starting points for exploring the implementation:

| Area | Source | What to look for |
| --- | --- | --- |
| Game coordination | [GameDirector.cs](Assets/Project/Scripts/Manager/GameDirector.cs) | State changes and manager coordination |
| Character control | [PlayerMovement.cs](Assets/Project/Scripts/Element/PlayerMovement.cs) | Movement, aiming, stamina, and dash timing |
| Enemy AI | [Enemy.cs](Assets/Project/Scripts/Element/Enemy.cs) | Visibility checks, navigation, and combat states |
| Weapons | [Weapon.cs](Assets/Project/Scripts/Element/Weapon.cs) | Fire timing, automatic reload, and projectile reuse |
| Projectile lifecycle | [Bullet.cs](Assets/Project/Scripts/Element/Bullet.cs) | Collisions and returning bullets to their pool |
| Grenade throwing | [GrenadeThrower.cs](Assets/Project/Scripts/Element/GrenadeThrower.cs) | Target selection, animation timing, and throw trajectory |
| Level progression | [LevelManager.cs](Assets/Project/Scripts/Manager/LevelManager.cs) | Prefab initialization and cycling through layouts |

The implementation uses direct component references, initialization methods, and shared manager instances. Working with these relationships helped me understand object lifecycles and the dependencies between game systems.

---

## 🕹️ Controls

| Input | Action |
| --- | --- |
| W / A / S / D | Move |
| Mouse | Aim |
| Left mouse button | Fire |
| Right mouse button | Throw a grenade |
| Left Shift | Sprint |
| Space | Dash in the main scene |
| 1 / 2 | Select machine gun / shotgun |
| Escape | Open the escape menu |
| R | Restart the level |

The current source also includes **P / O** shortcuts for advancing to the next level or returning to the previous one. Machine-gun reload is automatic when firing with an empty magazine; **R restarts the level**.

---

## 🛠️ Tools & Technologies

- **Unity 2022.3.62f3** and **C#**
- Universal Render Pipeline (**URP**)
- Unity Physics: Rigidbody, colliders, triggers, and raycasts
- AI Navigation / NavMeshAgent
- Animator, animation layers, and coroutines
- Unity UI and TextMeshPro
- DOTween
- Particle System, AudioSource, and VideoPlayer
- Git and GitHub Desktop

---

## 📂 Project Structure

```text
Assets/
├── Project/
│   ├── Scripts/
│   │   ├── Element/    # Characters, weapons, projectiles, and level objects
│   │   ├── Manager/    # Game, level, weapon, timer, audio, and FX coordination
│   │   └── UI/         # Menus, HUD, and feedback components
│   ├── Prefabs/
│   └── ...             # Animations, audio, materials, and other game resources
└── Scenes/
    └── MainScene.unity
Packages/
ProjectSettings/
```

---

## ▶️ Run the Project

1. Clone or download this repository.
2. Install **Unity 2022.3.62f3** through Unity Hub. Include Windows Build Support if you want to create a Windows build.
3. Add the repository folder to Unity Hub and open it.
4. Allow Unity to import assets and resolve the packages.
5. Open `Assets/Scenes/MainScene.unity`.
6. Enter Play mode and start the game from the main menu.

For a Windows build, use **File → Build Settings**, select the PC platform and Windows target, and build with `MainScene` enabled.

<!-- Add the author's exact itch.io game URL and gameplay media when supplied. -->

---

## 📚 Learning Resources & Credits

This project was developed with guidance from the **Bitten section (Section 5)** of [Sıfırdan Oyun Yazılımı ve Tasarımı Öğren! on Udemy](https://www.udemy.com/course/sifirdan-oyun-yazilimi-ve-tasarimi-ogren/), alongside independent research and experimentation.

The course provided the foundation for the project. The code and gameplay shown here reflect my learning, implementation work, and subsequent adjustments. Third-party models, audio, effects, and libraries are credited to their respective creators; I do not claim authorship of those resources. Their individual licenses apply.

## 👤 Author

**[Eren Yusuf Can](https://github.com/erenyusufcan)**  
Unity & C# game development portfolio
