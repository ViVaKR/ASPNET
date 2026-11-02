# AiChatServer


```bash
mkdir AiChatServer && cd AiChatServer
dotnet new sln -n AiChatServer

dotnet new classlib -n AiChatServer.Contracts -o src/AiChatServer.Contracts
dotnet new web      -n AiChatServer.Grpc      -o src/AiChatServer.Grpc

dotnet sln add src/AiChatServer.Contracts src/AiChatServer.Grpc
dotnet add src/AiChatServer.Grpc reference src/AiChatServer.Contracts

# Contracts: 서버/클라이언트가 같이 쓰는 계약
dotnet add src/AiChatServer.Contracts package Google.Protobuf
dotnet add src/AiChatServer.Contracts package Grpc.Core.Api
dotnet add src/AiChatServer.Contracts package Grpc.Tools

# Server
dotnet add src/AiChatServer.Grpc package Grpc.AspNetCore
dotnet add src/AiChatServer.Grpc package Grpc.AspNetCore.HealthChecks
dotnet add src/AiChatServer.Grpc package Grpc.AspNetCore.Server.Reflection


grpcurl -plaintext -d '{"conversation_id":"c1","persona":"PERSONA_BEOPDEUNG","messages":[{"role":"CHAT_ROLE_USER","content":"까꿍"}]}' localhost:8080 aichat.v1.ChatService/StreamChat
````
