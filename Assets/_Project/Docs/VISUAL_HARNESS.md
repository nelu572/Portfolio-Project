# Visual Harness

## Target Feel

- Mid-poly silhouettes with rough edges
- PSX-like low-fidelity presentation
- Deliberately unstable horror mood
- Steampunk machinery, pipework, pressure tanks, valves, metal seams

## Rules

1. Favor bold shapes over fine detail.
2. Textures should read clearly at low resolution.
3. Avoid clean modern surfaces. Prefer rust, grime, worn paint, oxidized metal, soot.
4. Lighting should feel unreliable. Use flicker, falloff, contrast pockets, and shadow-heavy corners.
5. Limit scene clutter to props that support navigation, defense, or dread.
6. Color palette should lean toward dirty brass, soot black, dried blood red, oil green, fog gray.
7. Models should remain readable from a distance even when rendered roughly.

## Runtime Harness

- `PsxVisualSettings` disables clean presentation defaults where possible and applies rougher camera settings.
- `FlickerLight` provides unstable point-light motion for horror beats.
- The harness scene includes basic pipes and a flickering warm light to keep the style direction visible even before art production.

## Art Review Checklist

- Does the asset still read clearly at low resolution?
- Does it look too clean or too modern?
- Does it reinforce steampunk horror rather than generic sci-fi?
- Can it survive heavy fog, dim light, and noisy presentation?
