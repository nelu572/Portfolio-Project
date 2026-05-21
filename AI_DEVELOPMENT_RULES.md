# AI Development Rules

1. Do not casually change an existing public API if a local extension point can solve the task.
2. Read the related files before editing a system.
3. Do not modify `Player`, `Weapon`, `Enemy`, `Defense`, and `Core` in one large sweep unless the feature truly crosses those boundaries.
4. Prefer extending existing managers and controllers before creating a parallel one.
5. Mark temporary shortcuts with `TODO:` and keep the note specific.
6. Finish work in a buildable state. Do not leave syntax errors behind.
7. Keep work testable inside `HarnessTestScene`.
8. Add features in the smallest runnable slice first, then expand.
9. Do not create duplicate service or manager layers without checking `Core` first.
10. If a value is likely to be tuned, move it into config/data instead of burying it in a method body.
11. If a file is already responsible for a concept, extend that file instead of scattering logic into unrelated classes.
12. Preserve the harness: avoid edits that make `HarnessTestScene` less reliable as a validation scene.
