# Runner Refactor Roadmap

## Goal
Transform the current prototype into a production-style Unity runner that:
- satisfies all lab requirements;
- keeps existing shop/skin progress;
- remains modular, testable, and easy to extend.

## Tech Stack
- Dependency Injection: VContainer
- Async flow: UniTask
- Optional animation layer: DOTween (for UI transitions and polish)

## Architecture Targets
- `Core`: game state, speed progression, score/coin session state.
- `Gameplay`: player movement, obstacle/coin spawning, collision outcomes.
- `UI`: HUD, main menu, game over, leaderboard.
- `Progression`: persistent save, records, unlocked/selected cosmetics.
- `Audio`: music and SFX buses.

## Migration Strategy
1. Keep current scene playable at all times.
2. Add systems in parallel and route old code to new services.
3. Remove legacy scripts only after parity checks.

## Phases

### Phase 1 - Foundation (in progress)
- Add DI composition root (`RunnerLifetimeScope`).
- Introduce `GameStateService`.
- Introduce `SpeedService` and time-based speed increase every 10 seconds.
- Make existing map movement consume centralized speed/state.

Definition of Done:
- Game starts in `Playing`.
- Speed increases over time without touching map logic manually.
- Core systems are no longer hidden inside random `Update` methods.

### Phase 2 - Core Gameplay Loop
- Add obstacle collision -> `GameOver`.
- Add restart flow.
- Add coin pickup pipeline (spawn, collect, despawn, wallet update).
- Connect HUD coin counter to runtime wallet/session.

Definition of Done:
- Player can collect coins.
- Collision ends run.
- Result screen appears with collected amount.

### Phase 3 - Player Controller Quality
- Replace fragile jump checks with robust grounded detection.
- Proper run/jump/fall animation state machine.
- Low vs high obstacles with explicit semantics.

Definition of Done:
- Jump timings feel consistent.
- Animation matches movement state.
- High obstacles cannot be jumped over.

### Phase 4 - Menus, Records, Persistence
- Main menu: Start / Records / Exit.
- Game over: enter name + save score.
- Leaderboard sorted descending and persisted.

Definition of Done:
- Full user flow from menu to run to record table.

### Phase 5 - Bonus & Dynamic Obstacles
- Bonus system (temporary coin multiplier or invulnerability).
- Moving obstacles (toward player and perpendicular).

Definition of Done:
- Bonus is visible, time-limited, and balanced.
- Moving obstacles are integrated into spawn system.

### Phase 6 - Customization Integration
- Reuse existing shop and skin selections in gameplay scene.
- Add visual options for platform/player/bonus styles.

Definition of Done:
- Selected skins are applied at runtime each session.

### Phase 7 - Polish & Stability
- Audio manager (music + coin SFX + game over SFX).
- UI transitions and feedback polish.
- Regression checklists and basic playmode tests.

Definition of Done:
- Lab requirements fully covered.
- Project remains stable after restarts and repeated runs.

## Risks
- Legacy scene wiring via inspector references.
- Existing shop code outside DI scope.
- Incomplete animator parameter schema.

## Mitigations
- Introduce adapter scripts first, then replace internals.
- Maintain save compatibility.
- Add small integration tests per phase.
