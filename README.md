# 프로젝트 및 팀 소개

## 팀 구성
- 준(June): 조장, 맵 전반적, Items
- 대석: PlayerStateMachine
- 하은: UI, SoundManager
- 동호: UI, PlayerStateMachine, BonusTime

## 프로젝트 소개
- 프로젝트: 쿠키런 모작
- 플랫폼: Android 및 iOS 어플리케이션
- 장르: 러닝 액션
- 목표: 다양한 능력을 가진 쿠키를 조종하여 최대한 멀리 가는 게임
- 주요 기술: State Pattern을 활용

## 프로젝트 진행

### 팀
- 프로젝트 진행 전 시스템 요구사항 및 역할 분담 회의
- 필요한 에셋은 Devsisters 공홈의 스프라이트 및 spriters-resource에서 사용
- 버전 관리는 GitHub 사용
- 격일 간의 팀 전체 회의를 통한 오류 및 추가사항 분석
- 개인별 작업 로그 기록

### 개인

#### 준(June)
- SpawnManager 구현
- 아이템 및 장애물, PanCake 쿠키 생성
- 무한 맵 구현
- 맵을 여러 겹으로 만들고 각각의 Scroll Speed를 다르게 할당하여 달리는 효과
- SpawnManager 구현
- 구글 스프레드 시트에서 작성된 파일을 JSON으로 변환하여 SpawnManager에서 값을 읽어오기

#### 대석
- PlayerStatePattern 구현
- 플레이어의 상태를 Idle, Jump, DoubleJump, Dash, Hit 등으로 관리
- 버튼 입력으로 움직임을 구현
- 각 상태에 따른 기능을 구현

#### 하은
- UI 자동화 구현
- 버튼 클릭, 다운, 업 이벤트 처리
- 캐릭터 선택, 구매, 장금 해제, 팝업창 구현
- LOBBY 화면의 쿠키 스크롤뷰 구현
- Excel에서 Json으로 데이터 변환 및 DataManager를 통한 데이터 연동

#### 동호
- UI 자동화 구현
- 버튼 이벤트 다양하게 처리
- 보너스 타임 시 연출 상태 구현
- 달빛 술사 캐릭터 구현
- 파티클 효과 구현
- SoundManager를 통한 음악 및 효과음 관리

## 프로젝트 장점 및 단점
- 장점:
    - StatePattern을 사용하여 플레이어의 상태를 관리, 레벨 디자인에 용이
    - SpawnManager를 통한 아이템 및 장애물 생성으로 패턴 다양화
    - DataManager를 활용하여 캐릭터 관리가 편리
- 단점:
    - 초기 설정 시 시간 소요
    - 맵 및 장애물을 프리팹화하지 않아 효율성이 떨어짐
