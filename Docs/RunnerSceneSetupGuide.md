# Runner Scene Setup Guide

This project now contains a modular runner architecture (VContainer + UniTask).
To enable all new systems in Unity scene, wire the references below once.

## 1) Composition Roots (3 scenes)
- `Bootstrap` scene:
  - create `BootstrapRoot` with `DontDestroyOnLoadAnchor` + `BootstrapLifetimeScope`.
  - set `SceneFlowSettings` names (`Bootstrap`, `Menu`, `Game`).
- `Menu` scene:
  - create `MenuScope` with `MenuLifetimeScope`.
- `Game` scene:
  - create `GameScope` with `GameLifetimeScope`.
  - tune `RunnerGameplaySettings` in inspector:
  - Initial Speed
  - Max Speed
  - Speed Increase Step
  - Speed Increase Interval Seconds
  - Start In Menu (usually off for dedicated `Game` scene)

## 2) Map and Pools
- On `MapGenerator`:
  - keep `chunksPool` and `obstaclePool` references;
  - assign `coinPool` (new);
  - assign `bonusPool` (new, optional);
  - assign `spawnSettings` (optional ScriptableObject);
  - set `minCoinRate` / `maxCoinRate`.
- Create coin prefabs with:
  - trigger collider;
  - `CoinPickup` component;
  - optional `Rotator`.
- Put coin prefabs under coin pool source list.

## 3) Player
- Ensure player has:
  - `Player`
  - `PlayerMovement`
  - `PlayerAnimator`
  - `PlayerInputSystem`
  - `CheckObstacle`
- Configure `CheckObstacle.obstacleLayerMask` to obstacle layers.

## 4) HUD and Game Over UI
- Add `RunHudView` to HUD root and assign:
  - coins text
  - speed text
  - multiplier text (optional)
- Add `GameOverView` and assign:
  - gameplay panel
  - game over panel
  - coins result text
  - player name input
  - save button
  - restart button
  - save state text (optional)
- Add `LeaderboardView` in records panel and assign:
  - `entriesRoot` (vertical layout container)
  - `entryPrefab` (`LeaderboardEntryView` prefab)
  - `emptyStateText` (optional)
- Optional polish:
  - add `CanvasGroup` + `UiPanelTransition` on menu/hud/gameover roots;
  - assign transitions to `GameStateUiRouter` and `MainMenuView`.

## 5) Audio
- Add `RunnerAudioController` to any scene object.
- Assign:
  - music source
  - sfx source
  - coin clip
  - game over clip

## 6) Optional Bonus System
- Create bonus prefab with trigger collider.
- Add `BonusPickup` and choose type:
  - CoinMultiplier
  - Invulnerability
- Configure duration and multiplier.
- Put bonus prefabs into `bonusPool`.

## 6.1) Spawn Tuning Asset
- Create asset: `Create -> Runner -> Map Spawn Settings`.
- Assign it to `MapGenerator.spawnSettings`.
- Tune chunk length, cadence for obstacle/coin/bonus, and spawn ranges without code edits.

## 7) Optional Moving Obstacles
- Add `MovingObstacle` to obstacle prefab.
- Choose mode:
  - TowardPlayer
  - PerpendicularOscillation

## 8) Main Menu
- Add `MainMenuView` to menu UI root.
- Buttons are bound in code, so assign button references in inspector:
  - `startButton`
  - `leaderboardButton`
  - `backFromLeaderboardButton`
  - `shopButton`
  - `backFromShopButton`
  - `exitButton`
- Assign panel roots:
  - `menuRoot`
  - `leaderboardRoot`
  - `shopRoot` (optional)
- For animated panel swaps:
  - assign `menuTransition`, `leaderboardTransition`, `shopTransition`.

## 9) Shop + Customization Integration
- Keep existing `ShopBootstrap`, it now reuses shared runtime data when available.
- Place shop UI under `shopRoot` and drive open/close via `MainMenuView` shop buttons.
- Add `VisualCustomizationApplier` in gameplay scene:
  - map selected character variants to `characterBindings`
  - map selected maze/platform variants to `mazeBindings`
  - map bonus visuals to `bonusStyleBindings`
- Optional: use `BonusStyleSelector` on UI buttons to switch bonus style and persist.
