# 빌드옵션

### 1. 로그 상세도 조절하기 (`/v:...` 또는 `--verbosity ...`)

오늘 우리가 터미널에서 톡톡히 써먹었던 그 옵션이죠! 빌드가 왜 터졌는지, 혹은 내 타겟이 왜 무시당했는지 파고들 때 필수입니다.

* **`q[uiet]`**: 에러 메시지만 무소의 뿔처럼 조용히 출력합니다.
* **`m[inimal]`**: 중요한 에러와 경고, 주요 빌드 성공 여부만 간결하게 보여줍니다. (기본값)
* **`n[ormal]`**: 기본보다 조금 더 자세한 빌드 흐름을 보여줍니다.
* **`d[etailed]`**: 오늘 우리가 썼던 옵션으로, 타겟 내부의 속성 흐름과 세부 실행 과정을 낱낱이 까발려 줍니다.
* **`diag[nostic]`**: **디버깅 끝판왕**. 터미널 화면이 터져나갈 정도로 온갖 내부 변수와 메모리 상태를 다 쏟아냅니다.

### 2. 깨끗하게 지우고 다시 시작하기 (`--no-incremental` 또는 `/p:ForceRebuild=true`)

닷넷 빌드는 기본적으로 변경된 파일만 골라서 빌드하는 '증분 빌드(Incremental Build)'를 해서 속도가 아주 빠릅니다. 하지만 때로는 캐시나 빌드 꼬임 때문에 완전히 새로 다 밀고 빌드해야 할 때가 있죠.

* **`dotnet build --no-incremental`**: 이전 빌드 결과를 무시하고 대상 타겟들을 처음부터 다시 싹 실행시킵니다.
* **`dotnet clean && dotnet build`**: 아예 산출물 폴더를 깨끗하게 청소하고 빌드하는 가장 확실한 정석 코스입니다.

### 3. 복원(Restore) 과정을 제어하는 옵션

프로젝트를 처음 받았거나 패키지가 바꼈을 때 쓰이는 필수 옵션들입니다.

* **`--no-restore`**: 이미 NuGet 패키지가 다 복원되어 있다는 가정하에, 빌드 시간을 아끼려고 복원 과정을 싹 건너뛰고 바로 컴파일로 진입합니다. (대규모 솔루션 빌드 속도 최적화의 핵심!)

### 4. 빌드 구성 및 타겟 프레임워크 강제 지정 (`-c`, `-f`)

솔루션 전체가 아니라 특정 환경만 골라서 때릴 때 유용합니다.

* **`-c Release` (또는 `--configuration Release`)**: 디버그 모드가 아니라 성능 최적화와 난독화가 들어가는 릴리스(Release) 모드로 빌드합니다.
* **`-f net10.0` (또는 `--framework net10.0`)**: 멀티 타겟팅이 걸려 있는 프로젝트에서 특정 TargetFramework 페이즈만 콕 집어서 빌드합니다.

### 5. 경고를 에러로 다스리기 (`/p:TreatWarningsAsErrors=true`)

코드에 잔잔한 경고(Warning) 조차 남아 있는 걸 용납하지 않고 싶을 때 씁니다.

* **`/p:TreatWarningsAsErrors=true`**: 빌드 도중 경고가 단 하나라도 발생하면 컴파일을 곧바로 실패(Error) 처리해 버립니다. 코드 품질을 엄격하게 관리할 때 사용

---

## dotnet build -h

```text

Description:
  .NET 작성기

사용법:
  dotnet build [<PROJECT | SOLUTION | FILE>...] [options]

인수:
  <PROJECT | SOLUTION | FILE>  작업할 프로젝트 또는 솔루션 또는 C#(파일 기반 프로그램) 파일입니다. 파일을 지정하지 않으면 명령이 현재 디렉터리에서 프로젝트 또는 솔루션을 검색합니다.

옵션:
  --ucr, --use-current-runtime         현재 런타임을 대상 런타임으로 사용합니다. [default: False]
  -f, --framework <FRAMEWORK>          빌드할 대상 프레임워크입니다. 대상 프레임워크는 프로젝트 파일에도 지정되어야 합니다.
  -c, --configuration <CONFIGURATION>  프로젝트 빌드에 사용할 구성입니다. 대부분의 프로젝트에서 기본값은 'Debug'입니다.
  --version-suffix <VERSION_SUFFIX>    프로젝트를 빌드할 때 사용할 $(VersionSuffix) 속성의 값을 설정합니다.
  --no-restore                         빌드하기 전에 프로젝트를 복원하지 마세요. [default: False]
  --interactive                        명령을 중지하고 사용자 입력 또는 작업을 기다리도록 허용합니다(예: 인증 완료). [default: True]
  -v, -verbosity <LEVEL>               MSBuild 세부 정보 표시 수준을 설정합니다. 허용되는 값은 q[uiet], m[inimal], n[ormal], d[etailed] 및 diag[nostic]입니다.
  -o, --output <OUTPUT_DIR>            빌드된 아티팩트를 배치할 출력 디렉터리입니다.
  --artifacts-path <ARTIFACTS_DIR>     아티팩트 경로입니다. 빌드, 게시 및 팩 출력을 포함한 프로젝트의 모든 출력이 지정된 경로 아래의 하위 폴더로 이동합니다.
  --no-incremental                     증분 빌드를 사용하지 않습니다. [default: False]
  --no-dependencies                    p2p(프로젝트 간) 참조를 빌드하지 않고 지정한 프로젝트만 빌드합니다. [default: False]
  -nologo, --no-logo                   시작 배너 또는 저작권 메시지를 표시하지 않습니다. [default: True]
  --sc, --self-contained               대상 컴퓨터에 런타임을 설치할 필요가 없도록 애플리케이션과 함께 .NET 런타임을 게시합니다.
                                       기본값은 'false'입니다. 그러나 .NET 7 이하를 대상으로 하고 런타임 식별자를 지정하는 경우 기본값은 'true'입니다. [default: False]
  --no-self-contained                  애플리케이션을 프레임워크 종속 애플리케이션으로 게시합니다. 애플리케이션을 실행하려면 호환되는 .NET 런타임이 대상 시스템에 설치되어 있어야 합니다. [default: False]
  -r, --runtime <RUNTIME_IDENTIFIER>   빌드할 대상 런타임입니다.
  -a, --arch <ARCH>                    대상 아키텍처입니다.
  --os <OS>                            대상 운영 체제입니다.
  --disable-build-servers              모든 영구 빌드 서버를 무시하도록 명령을 강제 실행합니다. [default: False]
  -?, -h, --help                       명령줄 도움말을 표시합니다.

  ```
