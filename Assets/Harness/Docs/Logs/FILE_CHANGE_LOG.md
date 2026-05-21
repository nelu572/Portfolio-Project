# 파일 변경 이력

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
