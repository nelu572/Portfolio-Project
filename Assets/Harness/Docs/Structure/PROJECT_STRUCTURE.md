# 프로젝트 구조

## 엔진 / 실행 환경

- 엔진: Unity 6 (`6000.0.68f1`)
- 렌더 파이프라인: URP
- 입력 시스템: Unity Input System
- 테스트 프레임워크: Unity Test Framework

## 현재 폴더 구조

- `Assets/Scenes`
  - `HarnessTestScene.unity`: 메인 하네스 테스트 씬
  - `SampleScene.unity`: Unity 기본 샘플 씬
- `Assets/Scripts`
  - `Core`: 공용 서비스, 부트스트랩, 설정, 저장, 씬 로딩
  - `Player`: 입력, 이동, 시점, 체력, 상호작용, 무기 제어
  - `Weapon`: 무기 베이스, 탄약, 장전, 히트스캔, 투사체 확장
  - `Enemy`: 좀비 AI, 체력, 공격, 스포너, 웨이브
  - `Defense`: 목표물, 바리케이드, 수리, 자원
  - `DebugTools`: 디버그 UI, 치트 키, 빠른 검증 도구
  - `Visual`: PSX/공포 분위기용 시각 보조
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

- 플레이 테스트 씬: `Assets/Scenes/HarnessTestScene.unity`
- 런타임 부트스트랩: `Assets/Scripts/Core/HarnessSceneBootstrapper.cs`
- 하네스 설치기: `Assets/Scripts/Core/HarnessSceneInstaller.cs`

## 씬 역할

- `HarnessTestScene`
  - 플레이어, 목표물, 좀비, 디버그 UI를 자동 구성하는 메인 검증 씬
  - 새 기능을 가장 먼저 확인하는 장소
- `SampleScene`
  - 참고용 예비 씬
  - 메인 검증 씬으로 사용하지 않음

## 책임 경계

- `Core`는 공용 서비스만 담당한다.
- `Player`는 적 생성이나 웨이브 규칙을 직접 담당하지 않는다.
- `Weapon`은 UI나 웨이브를 직접 관리하지 않는다.
- `Enemy`는 입력을 직접 읽지 않는다.
- `Defense`는 보호 대상, 수리, 자원 흐름을 담당한다.
- `DebugTools`는 테스트를 돕지만 본편 로직의 중심이 되면 안 된다.

## 빌드 / 실행

- Unity에서 `HarnessTestScene`를 열고 Play 한다.
- 빌드는 `File > Build Profiles` 흐름을 사용한다.
- 현재 빌드 시작 씬은 `HarnessTestScene`이다.
