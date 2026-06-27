# Todo App — 인증 기반 풀스택 Todo 관리

ASP.NET Core와 React로 구현한 JWT 인증 기반 Todo 관리 애플리케이션. 모던 스택(컨테이너·클라우드·CI/CD) 전환을 위한 기반 프로젝트로, 인증부터 배포까지 전 과정을 단독 설계·구현했다.

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)
![React](https://img.shields.io/badge/React-19-61DAFB?logo=react&logoColor=black)
![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?logo=typescript&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-18-4169E1?logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
![Azure](https://img.shields.io/badge/Azure_Container_Apps-0078D4?logo=microsoftazure&logoColor=white)

## 🔗 라이브 데모

**https://app-todo.braveocean-527033e9.westus2.azurecontainerapps.io**

아래 테스트 계정으로 바로 로그인해 둘러볼 수 있습니다:

| 이메일 | 비밀번호 |
|---|---|
| `test@gmail.com` | `1234` |

> ⚠️ 비용 절감을 위해 인스턴스가 0으로 축소됩니다(scale to zero). 첫 접속 시 10~30초의 콜드 스타트가 있을 수 있으며, 이후에는 정상 속도로 동작합니다.

## 📸 스크린샷

| 로그인 | 할 일 관리 |
|---|---|
| ![로그인](doc/images/login.png) | ![할 일 관리](doc/images/todos.png) |

## 개요

로그인한 사용자가 자신의 할 일을 관리하는 웹 애플리케이션이다. 단순한 CRUD 외에, 실무에서 사용하는 인증·데이터 격리·컨테이너 배포·CI/CD를 직접 구현하는 데 초점을 맞췄다. 의도적으로 단순한 도메인을 선택해, 아키텍처 과설계 없이 흐름(인증 → 사용자별 데이터 → 배포)에 집중했다. 이후 더 복잡한 메인 프로젝트로 확장하기 위한 기반이다.

## 아키텍처

**애플리케이션 구성**
```
Browser (React + TypeScript)
        │  HTTPS / JWT
        ▼
ASP.NET Core Web API  ──  정적 파일(React 빌드) 서빙
        │  EF Core
        ▼
PostgreSQL
```

**배포 파이프라인**
```
git push (main)
        │
        ▼
GitHub Actions  ──  멀티스테이지 Docker 빌드 (React → .NET)
        │  OIDC 인증 (시크릿리스)
        ▼
Azure Container Apps  ──  단일 컨테이너 (API + React)
```

## 기술 스택

| 영역 | 기술 |
|---|---|
| 백엔드 | ASP.NET Core Web API (.NET 9), Entity Framework Core 9 |
| 프론트엔드 | React 19, TypeScript, Vite, Tailwind CSS v4, axios, React Router v6 |
| 데이터베이스 | PostgreSQL 18 |
| 인증·보안 | JWT (JwtBearer), PasswordHasher (PBKDF2 + 솔트), `[Authorize]` 기반 접근 제어 |
| 인프라 | Docker (멀티스테이지 빌드), docker-compose |
| 배포 | Azure Container Apps, GitHub Actions (OIDC CI/CD) |

## 주요 기능

- **인증** — 회원가입 / 로그인, JWT 발급, 비밀번호 단방향 해싱
- **Todo CRUD** — 제목·상태·우선순위·마감일 관리
- **사용자별 데이터 격리** — 본인의 Todo만 접근 가능 (IDOR 방지)
- **보호 라우팅** — 미인증 시 로그인 화면으로 자동 리다이렉트
- **자동 토큰 첨부** — axios interceptor로 모든 요청에 JWT 자동 포함

## 실행 방법

### 방법 1: Docker Compose (권장)

전체 스택(앱 + DB)을 한 번에 실행한다.

```bash
docker-compose up --build
```

- 앱: http://localhost:8080 (React + API 통합 서빙)
- DB: PostgreSQL 컨테이너 (앱 시작 시 EF Core 마이그레이션 자동 적용)
- 첫 계정은 http://localhost:8080/swagger 의 `POST /api/auth/register`로 생성

종료:
```bash
docker-compose down      # 데이터 유지
docker-compose down -v   # 데이터 삭제
```

### 방법 2: 로컬 개발 (프론트/백 분리 실행)

**백엔드**
```bash
cd TodoApi
# User Secrets에 연결 문자열·JWT 키 설정 필요
dotnet run
# → http://localhost:5253 (Swagger: /swagger)
```

필요한 User Secrets:
```
ConnectionStrings:DefaultConnection = <PostgreSQL 연결 문자열>
Jwt:Key = <32자 이상 영문+숫자 비밀키>
```

**프론트엔드**
```bash
cd frontend
npm install
npm run dev
# → http://localhost:5173
```

## API 개요

| 메서드 | 엔드포인트 | 설명 | 인증 |
|---|---|---|---|
| POST | `/api/auth/register` | 회원가입 | — |
| POST | `/api/auth/login` | 로그인 (JWT 발급) | — |
| GET | `/api/todos` | 내 Todo 목록 | ✅ |
| POST | `/api/todos` | Todo 생성 | ✅ |
| PUT | `/api/todos/{id}` | Todo 수정 | ✅ |
| DELETE | `/api/todos/{id}` | Todo 삭제 | ✅ |

전체 API는 실행 후 `/swagger`에서 확인 및 테스트할 수 있다.

## 설계 노트

- **아키텍처**: 단순 CRUD라 계층형 선택.
- **데이터 격리**: 모든 쿼리에 사용자 ID 필터를 적용해 IDOR을 방지. 실제 보안은 백엔드가 담당하고 프론트 보호 라우팅은 UX 차원.
- **인증·토큰 저장**: axios interceptor로 JWT를 모든 요청에 자동 첨부. 저장 위치는 단순성을 위해 localStorage를 선택.
- **배포**: React를 ASP.NET Core에 합쳐 단일 컨테이너로. 멀티스테이지 빌드 + GitHub Actions 자동 배포, 컨테이너 시작 시 마이그레이션 자동 적용.

## 프로젝트 구조

```
todo-app/
├── TodoApi/                 # ASP.NET Core 백엔드
│   ├── Controllers/         # API 엔드포인트
│   ├── Models/              # 엔티티 (User, Todo)
│   ├── Dtos/                # 데이터 전송 객체
│   ├── Data/                # DbContext
│   ├── Services/            # TokenService 등
│   ├── Migrations/          # EF Core 마이그레이션
│   ├── wwwroot/             # (빌드 시) React 정적 파일
│   └── Dockerfile           # 멀티스테이지 빌드
├── frontend/                # React 프론트엔드
│   └── src/
│       ├── api/             # axios client, 타입 정의
│       ├── pages/           # LoginPage, TodoPage
│       └── components/      # ProtectedRoute
├── .github/workflows/       # GitHub Actions CI/CD
├── docker-compose.yml       # 로컬 통합 실행
└── docs/images/             # 스크린샷
```

> 이 프로젝트는 더 복잡한 메인 프로젝트로 가기 위한 기반이다. 다음 단계에서는 멀티테넌시, 실시간 기능, 클린 아키텍처 등 복잡도가 높은 요구사항을 적용하며 확장할 예정이다.
