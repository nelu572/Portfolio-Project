# 파일 변경 이력

## 메모

### 규칙

- `WORK_ORDER.md`에는 메모를 적지 않는다.
- 작업 지시와 별개인 메모는 이 문서의 메모 파트에 기록한다.
- 변경 이력과 작업 메모를 한 문서 안에서 함께 관리한다.

### 누적 메모

#### 2026-05-22

- `WORK_ORDER.md`는 템플릿 전용 문서로 유지한다.
- 사용자가 지우라고 할 때만 상단 안내와 템플릿을 남기고 작업 내용 영역을 비운다.
- `Docs`는 성격별 하위 폴더로 나눠 관리한다.

##### 문서 점검 메모

- `Assets/Harness/AGENTS.md`
  - 아직 `HarnessTestScene` 중심 검증을 기준으로 적혀 있다.
  - 현재 구조는 `HarnessTestScene`이 삭제됐고 `SampleScene`만 남아 있어 문서와 현재 상태가 다르다.
- `Assets/Harness/Docs/Planning/VISUAL_HARNESS.md`
  - `PsxVisualSettings`, `FlickerLight`, 하네스 씬 기본 파이프/조명 같은 런타임 하네스 설명이 남아 있다.
  - 현재는 관련 구현과 하네스 씬이 제거된 상태라 문서가 옛 상태를 기준으로 설명한다.
- `Assets/Harness/Docs/Logs/FILE_CHANGE_LOG.md`
  - 최근 이력으로는 맞지만, 현재 존재하지 않는 파일 이름이 여러 번 강조되어 있어 지금 구조를 빠르게 파악할 때 혼란을 줄 수 있다.

##### 방향성 충돌

- `README.md`
  - 이 저장소는 프로토타입 개발용이고, 완성형 출시 버전은 나중에 별도 저장소로 간다는 방향이다.
- `Assets/Harness/AGENTS.md`
  - 목표를 여전히 “출시 가능한 유지보수 구조 유지”로 강하게 두고 있다.
  - 프로토타입 저장소 기준인지, 출시 버전 전초기지 기준인지 톤을 하나로 맞출 필요가 있다.
- `Assets/Harness/Docs/Planning/TODO_ROADMAP.md`
  - 14단계까지 출시 준비를 현재 저장소 기준처럼 읽히게 적혀 있다.
  - README의 “프로토타입 저장소” 방향과 완전히 충돌하진 않지만, 같은 저장소에서 출시까지 가는 것처럼 오해될 수 있다.
- `Assets/Harness/Docs/Conventions/GITHUB_HARNESS.md`
  - `develop` 기반 Git-Flow, 이슈 번호 포함 커밋, 작업 브랜치 규칙을 강하게 요구한다.
  - 실제 지금 작업 흐름은 `main` 직커밋 중심으로 진행되어 문서와 운영 방식이 다르다.

##### VISUAL_HARNESS가 빡빡한 이유

- 색 팔레트를 `더러운 황동`, `마른 피의 붉은색`, `기름 낀 초록`처럼 거의 확정 룩으로 못 박고 있다.
- 조명을 “믿을 수 없어 보여야 한다”처럼 강한 표현으로 고정해, 프로토타입 단계에서 다른 톤 실험 여지를 줄인다.
- 배경 오브젝트 기준도 너무 기능 위주라, 분위기 실험용 장식을 넣기 어렵게 읽힌다.
- 현재 구현이 없는 상태인데도 런타임 하네스까지 규정하고 있어, 문서가 방향 제안이 아니라 강한 제약처럼 보인다.

##### 정리 방향 후보

- `AGENTS.md`
  - `HarnessTestScene` 기준 문구 삭제
  - 저장소 목적을 “프로토타입 개발용”으로 맞추기
- `VISUAL_HARNESS.md`
  - 규칙 문서를 “강제 규칙”보다 “권장 방향” 중심으로 완화
  - 런타임 하네스 설명은 제거하거나 “나중에 다시 붙일 예정”으로 바꾸기
- `TODO_ROADMAP.md`
  - 현재 저장소는 프로토타입 검증용이라는 점을 한 줄 추가
  - 출시 준비 단계는 “후속 프로젝트/출시 버전으로 이어질 수 있음” 정도로 표현 완화
- `GITHUB_HARNESS.md`
  - 실제 운영을 `main` 중심으로 갈지, 문서대로 `develop` 브랜치 전략으로 갈지 먼저 결정 필요

#### 2026-05-23

- GitHub issue 생성 시도는 `403 Resource not accessible by integration`으로 실패했다.
- 이번 작업에서는 로컬 브랜치 `feature/player-movement`만 생성했고, 이슈 번호 연동은 하지 못했다.

##### 애매모호한 표현

- `README.md`
  - `게임의 핵심 재미를 다듬는 것`:
    - 전투 감각 검증인지, 전체 게임 루프 검증인지 범위가 넓다.
  - `완성형 출시 버전은 이후 별도의 저장소에서 새롭게 정리해 이어갈 예정`:
    - 언제 분리하는지, 이 저장소가 어디까지 담당하는지 불명확하다.
  - `스팀펑크 감성`, `거친 공포 연출`:
    - 분위기 방향은 보이지만 실제 작업 기준으로 쓰기엔 추상적이다.
- `AGENTS.md`
  - `빠르게 기능을 만들되, 출시 가능한 유지보수 구조를 유지한다`:
    - 지금 프로토타입 저장소 기준인지, 최종 제품 기준인지 해석이 갈릴 수 있다.
  - `작은 게임에 맞는 단순한 구조`:
    - 어느 정도까지 단순해야 하는지 기준이 없다.
- `PROJECT_STRUCTURE.md`
  - `현재 기본 씬`:
    - 임시 기본 씬인지, 당분간 메인 작업 씬인지 모호하다.
  - `출발점으로 사용 가능`:
    - 권장인지, 그냥 가능성 설명인지 모호하다.
- `TODO_ROADMAP.md`
  - `새 구조에서 다시 시작할 준비`:
    - 문서 정리만 끝난 것인지, 실제 구현 착수 가능 상태까지 포함하는지 애매하다.
  - `첫 무기 패스`, `첫 좀비 원형`:
    - 어느 수준까지 만들면 완료로 볼지 기준이 없다.
- `VISUAL_HARNESS.md`
  - `읽기 쉬운 형태`, `지나치게 깨끗하고 현대적인 표면`, `믿을 수 없어 보여야 한다`:
    - 해석 범위가 너무 넓어서 사람이나 AI마다 결과가 크게 달라질 수 있다.
  - `기여하는 것만 남긴다`:
    - 장식적 요소를 얼마나 허용하는지 불명확하다.
- `GITHUB_HARNESS.md`
  - `배포 가능한 안정 버전`:
    - 이 저장소 기준인지, 후속 출시 저장소 기준인지 모호하다.
  - `검증 후 develop으로 병합`:
    - 어떤 검증을 의미하는지 정해져 있지 않다.

## 2026-05-23

### 작업

- 기본 FPS 플레이어 이동 구현

### 변경 내용

- `feature/player-movement` 브랜치를 생성
- `GameScene`을 추가하고 FPS 이동 테스트용 씬 진입점을 구성
- `CharacterController` 기반 플레이어 이동과 마우스 시점 조작 스크립트를 추가
- 기존 `InputSystem_Actions.inputactions`를 재사용하는 입력 래퍼를 추가
- 바닥, 벽, 경사, 계단 테스트 지형을 `GameScene`에 직접 배치하는 방식으로 구성
- Unity 6 대응 `ProBuilder 6.0.8` 패키지 의존성을 추가
- GitHub 이슈 생성은 integration 권한 부족으로 실패하여 실제 생성하지 못함
- `GameSceneInstaller`를 제거하고 씬 진입점 자동 조립 방식을 사용하지 않도록 정리
- `FpsPlayerInput`, `FpsPlayerMotor`에서 불필요한 namespace를 제거

### 주요 파일

- `Assets/Scenes/GameScene.unity`
- `Assets/Scripts/Player/FpsPlayerInput.cs`
- `Assets/Scripts/Player/FpsPlayerMotor.cs`
- `Packages/manifest.json`
- `Packages/packages-lock.json`
- `ProjectSettings/EditorBuildSettings.asset`

## 2026-05-22

### 작업

- 폴더 정리 및 하네스 수정
- HarnessTestScene 수정

### 변경 내용

- `Assets/_Project` 기반 스크립트를 `Assets/Scripts`로 이동
- 하네스 문서를 `Assets/Harness/Docs`로 이동
- GitHub 하네스 문서 추가
- 코딩 컨벤션 문서 추가
- `AGENTS.md` 추가
- 변경 이력 문서 추가
- 빌드 검증을 위해 `Assembly-CSharp.csproj`의 스크립트 경로를 새 구조에 맞게 보정
- `AGENTS.md`, `WORK_ORDER.md`를 `Assets/Harness`로 이동
- `Docs` 폴더를 `Conventions`, `Planning`, `Logs`, `Structure`로 재정리
- `WORK_ORDER.md`를 템플릿 전용 문서로 복원
- 메모 전용 문서 분리 규칙 추가
- `HarnessTestScene`이 씬에 배치된 설치기 기준으로 동작하도록 변경
- `HarnessSceneInstaller`가 씬 오브젝트를 우선 참조하고, 부족한 요소만 보조 생성하도록 구조 변경
- 씬 오브젝트 이름을 영어 기준으로 통일하고, 바닥/벽/파이프/목표물을 씬에 직접 배치하는 테스트 구조로 정리
- 에디터에서도 씬 오브젝트가 바로 보이도록 `HarnessSceneInstaller`에 프리뷰 재생성 동작 추가
- 하네스 문서는 유지하고, `Assets/Scripts` 내부 구현 파일과 `HarnessTestScene`을 제거한 뒤 큰 폴더 구조만 남기도록 정리
- 현재 상태 문서와 로드맵을 구현 리셋 이후 기준으로 갱신
- 메모 기준으로 문서 간 충돌을 정리하고, 프로토타입 저장소 방향에 맞게 AGENTS / 로드맵 / GitHub / 비주얼 문서를 수정

### 주요 파일

- `Assets/Scripts/Core/HarnessSceneInstaller.cs`
- `Assets/Scripts/DebugTools/DebugOverlay.cs`
- `Assets/Harness/Docs/Structure/PROJECT_STRUCTURE.md`
- `Assets/Harness/Docs/Conventions/GITHUB_HARNESS.md`
- `Assets/Harness/Docs/Conventions/CODING_CONVENTIONS.md`
- `Assets/Harness/AGENTS.md`
- `Assets/Harness/WORK_ORDER.md`
- `Assets/Harness/Docs/Logs/WORK_MEMO.md`
- `Assembly-CSharp.csproj`
- `Assets/Scripts/Core/HarnessSceneInstaller.cs`
- `Assets/Scripts/Core/HarnessSceneBootstrapper.cs`
- `Assets/Scenes/HarnessTestScene.unity`
- `ProjectSettings/EditorBuildSettings.asset`
