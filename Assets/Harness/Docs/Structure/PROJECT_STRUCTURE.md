# 프로젝트 구조

## 엔진 / 실행 환경

- 엔진: Unity 6 (`6000.0.68f1`)
- 렌더 파이프라인: URP
- 입력 시스템: Unity Input System
- 테스트 프레임워크: Unity Test Framework

## 현재 폴더 구조

- `Assets/Scenes`
  - `SampleScene.unity`: Unity 기본 샘플 씬
- `Assets/Scripts`
  - `Core`: 공용 시스템용 최상위 폴더
  - `Player`: 플레이어 시스템용 최상위 폴더
  - `Weapon`: 무기 시스템용 최상위 폴더
  - `Enemy`: 적 시스템용 최상위 폴더
  - `Defense`: 디펜스 시스템용 최상위 폴더
  - `DebugTools`: 디버그 도구용 최상위 폴더
  - `Visual`: 비주얼 시스템용 최상위 폴더
- `Assets/Harness`
  - 하네스 전용 자산 및 하네스 문서 보관 폴더
- `Assets/Harness/Docs`
  - 하네스 문서 루트
  - `Conventions`: 작업 규칙, 코딩 규칙, GitHub 규칙
  - `Planning`: 로드맵, 비주얼 방향, 중장기 방향 문서
  - `Logs`: 변경 이력, 작업 메모
  - `Structure`: 구조 설명 문서
- `Assets/Harness/WORK_ORDER.md`
  - AI 작업 지시 템플릿
- `Assets/Harness/AGENTS.md`
  - AI가 먼저 읽어야 하는 프로젝트 운영 안내서

## 주요 진입점

- 현재 남아 있는 씬: `Assets/Scenes/SampleScene.unity`
- 하네스 문서 루트: `Assets/Harness`

## 씬 역할

- `SampleScene`
  - 현재 기본 씬
  - 이후 새 테스트 씬을 다시 만들 때 출발점으로 사용 가능

## 책임 경계

- 현재는 폴더 경계만 유지한다.
- 새 기능을 다시 만들 때는 각 폴더 책임을 넘지 않도록 한다.

## 빌드 / 실행

- Unity에서 `SampleScene`를 열고 Play 한다.
- 빌드는 `File > Build Profiles` 흐름을 사용한다.
- 현재 빌드 시작 씬은 `SampleScene`이다.
