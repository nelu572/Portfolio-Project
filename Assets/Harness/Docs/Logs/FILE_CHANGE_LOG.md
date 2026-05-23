# 파일 변경 이력

## 현재 커밋 안 된 작업

## 2026-05-23

### 작업 1: 플레이어 이동 브랜치 준비

#### 변경 내용

- `feature/player-movement` 브랜치를 생성.
- GitHub 이슈 생성을 시도했으나 `403 Resource not accessible by integration` 권한 오류로 실패.
- 이슈 번호 연동 없이 로컬 브랜치 기준으로 작업 진행.

#### 주요 파일

- 없음

### 작업 2: 기본 이동 테스트 씬 구성

#### 변경 내용

- `GameScene`을 추가하고 FPS 이동 테스트용 씬 진입점을 구성.
- 바닥, 벽, 경사, 계단 테스트 지형을 씬에 직접 배치.
- 씬 진입점 자동 조립용 `GameSceneInstaller` 방식은 사용하지 않도록 정리.
- `GameScene`을 빌드 설정에 추가.

#### 주요 파일

- `Assets/Scenes/GameScene.unity`
- `Assets/Scenes/GameScene.unity.meta`
- `ProjectSettings/EditorBuildSettings.asset`

### 작업 3: 플레이어 입력/이동 구현

#### 변경 내용

- `CharacterController` 기반 플레이어 이동과 마우스 시점 조작을 구현.
- 이동, 점프, 스프린트, 시점 입력을 받는 플레이어 입력 래퍼를 추가.
- 기존 `InputSystem_Actions.inputactions`를 재사용.
- `PlayerRoot`에 Unity Input System의 `PlayerInput` 컴포넌트를 추가하고 `Player` 액션 맵을 연결.
- 입력 처리는 `PlayerInput + Invoke Unity Events` 기반으로 정리.

#### 주요 파일

- `Assets/Scripts/Player/PlayerInput.cs`
- `Assets/Scripts/Player/PlayerInput.cs.meta`
- `Assets/Scripts/Player/PlayerMotor.cs`
- `Assets/Scripts/Player/PlayerMotor.cs.meta`

### 작업 4: 패키지 의존성 추가

#### 변경 내용

- Unity 6 대응 `ProBuilder 6.0.8` 패키지 의존성을 추가.
- ProBuilder 프로젝트 설정 파일을 추가.

#### 주요 파일

- `Packages/manifest.json`
- `Packages/packages-lock.json`
- `ProjectSettings/Packages/com.unity.probuilder/Settings.json`

### 작업 5: 하네스와 코드 컨벤션 정리

#### 변경 내용

- 플레이어 입력/이동 스크립트에서 불필요한 namespace를 제거.
- 필수 이유 없는 `Application.isPlaying` 실행 상태 방어 조건을 피하는 기준을 코딩 컨벤션에 추가.
- 플레이어 이동 스크립트의 불필요한 `Application.isPlaying` 체크를 제거.
- FPS 접두어를 제거해 플레이어 입력/이동 클래스명을 `PlayerInput`, `PlayerMotor`로 변경.
- 클래스명 변경에 맞춰 파일명을 `PlayerInput.cs`, `PlayerMotor.cs`로 변경.
- 빈 스크립트 폴더 추적용 `.gitkeep` 파일을 제거.

#### 주요 파일

- `Assets/Harness/Docs/Conventions/CODING_CONVENTIONS.md`
- `Assets/Harness/Docs/Logs/FILE_CHANGE_LOG.md`
- `Assets/Scripts/Core/.gitkeep`
- `Assets/Scripts/DebugTools/.gitkeep`
- `Assets/Scripts/Defense/.gitkeep`
- `Assets/Scripts/Enemy/.gitkeep`
- `Assets/Scripts/Player/.gitkeep`
- `Assets/Scripts/Visual/.gitkeep`
- `Assets/Scripts/Weapon/.gitkeep`

### 작업 6: 플레이어 커서 잠금 디버그 토글 추가

#### 변경 내용

- `PlayerMotor`가 `lockCursorOnPlay` 값을 기준으로 커서 잠금/표시 상태를 적용하도록 정리.
- 디버그 전용 입력 처리를 `PlayerMotorCursorLockDebugToggle` 클래스로 분리.
- `L` 키를 누르면 디버그 컴포넌트가 `PlayerMotor.LockCursorOnPlay` 값만 반전하도록 구현.
- 디버그 컴포넌트를 `GameScene`의 `PlayerRoot`에 연결.
- 나중에 디버그 기능 제거 시 디버그 컴포넌트와 스크립트만 제거하면 되도록 토글 실행 로직을 분리.

#### 주요 파일

- `Assets/Scripts/Player/PlayerMotor.cs`
- `Assets/Scripts/DebugTools/PlayerMotorCursorLockDebugToggle.cs`
- `Assets/Scripts/DebugTools/PlayerMotorCursorLockDebugToggle.cs.meta`
- `Assets/Scenes/GameScene.unity`
