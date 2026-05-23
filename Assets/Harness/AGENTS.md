# AGENTS 안내서

## 프로젝트 개요

- 프로젝트명: Portfolio-Filling
- 장르: FPS / 좀비 디펜스 / 스팀펑크 / 공포
- 목표: 프로토타입 단계에서 FPS 전투, 좀비 디펜스, 비주얼 방향을 빠르게 검증할 수 있는 구조를 유지한다.

## 먼저 읽을 문서

1. `Assets/Harness/WORK_ORDER.md`
2. `Assets/Harness/Docs/Structure/PROJECT_STRUCTURE.md`
3. `Assets/Harness/Docs/Conventions/AI_DEVELOPMENT_RULES.md`
4. `Assets/Harness/Docs/Conventions/CODING_CONVENTIONS.md`
5. 필요 시 `Assets/Harness/Docs/Conventions/GITHUB_HARNESS.md`

## 문서 맵

- `Assets/Harness/WORK_ORDER.md`: 현재 작업 지시
- `Assets/Harness/Docs/Structure/PROJECT_STRUCTURE.md`: 폴더 구조와 책임
- `Assets/Harness/Docs/Conventions/AI_DEVELOPMENT_RULES.md`: AI 작업 규칙
- `Assets/Harness/Docs/Conventions/CODING_CONVENTIONS.md`: 코드 작성 규칙
- `Assets/Harness/Docs/Conventions/GITHUB_HARNESS.md`: 브랜치/커밋/이슈 규칙
- `Assets/Harness/Docs/Planning/TODO_ROADMAP.md`: 출시 로드맵
- `Assets/Harness/Docs/Planning/VISUAL_HARNESS.md`: 비주얼 방향
- `Assets/Harness/Docs/Logs/FILE_CHANGE_LOG.md`: 최근 수정 이력

## 프로젝트 의도

- 프로토타입 개발에 맞는 단순한 구조를 유지한다.
- AI가 기능을 추가할 때 어디를 수정해야 하는지 바로 보이게 한다.
- 현재는 `SampleScene` 또는 이후 새로 만드는 테스트 씬을 기준으로 빠르게 검증할 수 있어야 한다.
- Player, Weapon, Enemy, Defense, Core의 책임 경계를 명확히 유지한다.

## 작업 규칙

- 현재 프롬프트가 `Assets/Harness/WORK_ORDER.md`보다 우선한다.
- 지시가 없다면 TODO를 남기기보다 먼저 동작하는 구현을 만든다.
- 작업이 끝나면 `Assets/Harness/Docs/Logs/FILE_CHANGE_LOG.md`를 갱신한다.
- 사용자가 `메모 작업해`라고 요청하면 `Assets/Harness/WORK_ORDER.md`를 먼저 읽고, 그 안에 적힌 작업 지시를 실제 작업으로 수행한다.
- 사용자가 `메모 작업 n번째 해`처럼 번호를 지정하면 `Assets/Harness/WORK_ORDER.md`의 상세 요구사항 중 해당 번호만 수행한다.
- `메모 작업해`는 단순 메모 정리가 아니라, `Assets/Harness/WORK_ORDER.md` 기반 작업 시작 명령으로 해석한다.
- `Assets/Harness/WORK_ORDER.md`는 템플릿 파일이므로 작업 내용 영역은 커밋 전 비워진 상태를 유지하고, 커밋/푸시 대상에 포함하지 않는다.
- `Assets/Harness/WORK_ORDER.md` 맨 아래의 `유저 메모장` 파트는 작업 대상으로 해석하지 않고, 비우거나 지우거나 커밋하지 않는다.
- `Assets/Harness/Docs/Logs/FILE_CHANGE_LOG.md`의 `현재 커밋 안 된 작업` 섹션은 커밋 전 비워진 상태를 유지하고, 커밋/푸시 대상에 포함하지 않는다.
- 로그에 작업을 나열할 때는 최근에 수행한 작업을 위에 적는다.

## 문서 처리 규칙

- Markdown 문서는 UTF-8로 저장합니다.
- PowerShell에서 Markdown을 읽을 때는 `Get-Content -Encoding UTF8`을 사용합니다.
- 한글 출력이 깨지면 `[Console]::OutputEncoding = [System.Text.Encoding]::UTF8`을 먼저 설정합니다.
