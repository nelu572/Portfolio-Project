# Project Structure

## Engine / Runtime

- Engine: Unity 6 (`6000.0.68f1`)
- Render Pipeline: URP
- Input: Unity Input System package installed
- Test Framework: Unity Test Framework package installed

## Current Layout

- `Assets/Scenes`
  - `SampleScene.unity`: untouched Unity sample scene kept as a safe baseline
  - `HarnessTestScene.unity`: AI-facing gameplay harness scene for rapid validation
- `Assets/_Project/Scripts/Core`
  - Shared bootstrap, registry, save/config, scene loading, common interfaces
- `Assets/_Project/Scripts/Player`
  - FPS player input, movement, look, health, interaction, weapon control
- `Assets/_Project/Scripts/Weapon`
  - Weapon base classes, hitscan/projectile stubs, ammo and reload logic
- `Assets/_Project/Scripts/Enemy`
  - Zombie enemy root, health, movement, attack, spawn, wave loop
- `Assets/_Project/Scripts/Defense`
  - Objective, barricade, repair/build hooks, resource holder
- `Assets/_Project/Scripts/DebugTools`
  - Overlay UI, FPS/debug state, wave buttons, cheat keys
- `Assets/_Project/Scripts/Visual`
  - PSX-style runtime visual rules and light instability helpers

## Scene Responsibility

- `HarnessTestScene`
  - Main gameplay validation scene
  - Auto-builds a floor, walls, player, objective, zombie spawner, debug overlay
  - Safe place for AI to validate one feature at a time
- `SampleScene`
  - Kept as a reference / fallback scene
  - Do not use as the main gameplay test scene

## Main Entry Points

- Unity Play Mode entry: `Assets/Scenes/HarnessTestScene.unity`
- Runtime bootstrap: `Assets/_Project/Scripts/Core/HarnessSceneBootstrapper.cs`
- Scene installer: `Assets/_Project/Scripts/Core/HarnessSceneInstaller.cs`

## Responsibility Boundaries

- `Core` owns shared services only
- `Player` should not spawn enemies directly
- `Weapon` should not manage waves or UI
- `Enemy` should not read raw input
- `Defense` owns protect/repair/resource logic
- `DebugTools` may call into systems for testing, but should not become production gameplay logic

## Build / Run

- Open project in Unity 6
- Open `HarnessTestScene` or press Play with it set as the active scene
- Build target is still standard Unity build flow from `File > Build Profiles`
- Current build list is scene-based and starts from `HarnessTestScene`
