# 파일 변경 이력

## 2026-05-22

### 작업

- 폴더 정리 및 하네스 수정

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
