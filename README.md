# ASP.NET

### 웹앱 프로젝트 만들기
> macOS DOTNET WebApp Environment  

```bash
    # Create New Project (프로젝트 생성)
    dotnet new webapp
    
    # HTTPS Settings 
    dotnet dev-certs https --trust
    
    # First Run Test (최초 실행 테스트) 
    dotnet watch run
    
    # By vscode editor, develop (개발)...
    $ code .
    
    # publish (배포)
    dotnet publish --configuration Release
    # default location (배포판 기본위치): .../bin/release/net<version>/publish/
    # copy to web service location (웹 서비스 위치로 복사하기)
    
    # Server app test (웹 서비스 위치에서 배포판 테스트)
    dotnet <app_assembly>.dll
    # browser test (e.g. http://localhost:5000)
    # http://<ServerAddress>:<Port>
```
> 맥 서비스 파일 만들기 (macOS Plist) : dll 등록
1. Create plist file : /Library/LaunchDaemons/<domain>.plist
2. plist example
```xml
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
    <dict>
        <key>GroupName</key>
        <string>wheel</string>
        <key>InitGroups</key>
        <true/>
        <key>KeepAlive</key>
        <true/>
        <key>Label</key>
        <string><!-- 서비스이름 (DomainNameWebApp)--></string>
        <key>ProgramArguments</key>
        <array>
            <!-- $ which dotnet info -->
            <string>/usr/local/bin/dotnet</string>
            <string><!-- AssemblyName.dll (domain.dll) --></string>
        </array>
        <key>RunAtLoad</key>
        <true/>
        <key>UserName</key>
        <string><!-- 로그인 계정명 ($whoami) --></string>
        <key>WorkingDirectory</key>
        <string><!-- Web Service Root Folder Path --></string>
    </dict>
</plist>
```
3. Service Test (서비스 파일 이상유무 확인하기)
```bash
    # load
    launchctl load -w /Library/LaunchDaemons/<fileName>.plist
    # check 
    launchctl list | grep <plist Label : Service Name>
    # unload
    launchctl unload /Library/LaunchDaemons/<fileName>.plist
```

4. Start Always : 
   1. Create By Automator Shell Script -> 응용프로그램에 등록
   2. 로그인 항목에 등록

## Docker Nginx Hosting
>  준비중 ...
