초안을 파일로 만들어 볼게요. 새 proto 기준이고, 클라이언트 패키지는 서버 구현(Ollama 등)을 전혀 모르게 했어요.

초안 만들어 뒀어요, 제독님 ⚓ 컴파일은 못 돌려봤으니 로컬에서 한 번 빌드해 보세요.

## 구성

- **`IAiChatClient`**: `CompleteAsync`(단발)와 `StreamAsync`(토큰 스트림) 두 개가 전부예요. 사이트 코드는 이것만 알면 됩니다.
- **`AiChatModels.cs`**: `AiTurn`, `AiChunk`, `AiUsage` 같은 경계 타입이에요. proto 생성 타입이 Chat 서버 코드로 새어 나가지 않아서, proto가 바뀌어도 `GrpcAiChatClient` 한 곳만 고치면 됩니다.
- **`AiChatClientOptions`**: `Address`, `Persona`, 타임아웃 2개, `MaxHistory`가 들어 있어요. `ValidateOnStart`가 걸려 있어서, 주소가 틀리거나 Persona를 빼먹으면 파드가 시작 단계에서 바로 죽어요.
- **`GrpcAiChatClient`**: 이번 질문을 항상 마지막 user 메시지로 붙이고, 히스토리는 `MaxHistory`만큼 최신 것만 보내요. 빈 질문은 gRPC 호출 전에 막고요.
- **`ServiceCollectionExtensions`**: gRPC 클라이언트 등록에 keep-alive 설정이 들어 있어요. 스트림이 길어질 때 유휴 연결이 끊기는 걸 막아요.

## 사이트에서 쓰는 모습

```csharp
// buddham Chat 의 Program.cs
builder.Services.AddAiChatClient(o =>
{
    o.Address = builder.Configuration["Services:Ai:Address"]!; // env: Services__Ai__Address
    o.Persona = Persona.Beopdeung;                             // 사이트마다 이 줄만 다름
});
```

```csharp
// 허브나 오케스트레이터에서
await foreach (var c in ai.StreamAsync(conversationId, question, history, ct))
{
    if (c.Kind == AiChunkKind.Thinking) { /* 접어서 표시 */ }
    if (c.Done) break;
}
```

기존 `ChatOrchestrator`에 붙일 때는 `IAnswerStreamer` 자리에 `IAiChatClient`를 주입하고, `AiChunk`를 `AnswerChunk(Delta, Kind == Thinking)`로 바꿔주는 어댑터 한 줄이면 돼요.

## 확인해 주세요

1. **ContractsProject 경로**: csproj의 `..\AiChat.Contracts\AiChat.Contracts.csproj`는 제가 추측한 이름이에요. 실제 프로젝트명에 맞춰 고쳐주세요.
2. **enum 이름**: `Persona.Beopdeung`, `ChunkKind.Thinking`은 proto의 `PERSONA_BEOPDEUNG`, `CHUNK_KIND_THINKING`에서 C# 생성기가 접두어를 떼어낸 이름이에요. 서버의 `ChatRole.Assistant`, `ChunkKind.Content`와 같은 규칙이라 맞을 거예요.
3. **스트림 마감 시간 600초**는 nginx의 `proxy_read_timeout`과 맞춘 값이에요. 클러스터 내부 호출이면 상관없지만, nginx를 거치게 되면 같이 조정하세요.

## 일부러 안 넣은 것

- **예외 변환**: `RpcException`(`Unavailable` 등)을 그대로 던져요. C#은 `yield return`이 있는 메서드 안에서 `try/catch`를 못 써서, 필요하면 Chat 서버 쪽 허브나 컨트롤러에서 한 번에 잡는 게 깔끔해요.
- **인증**: 서버가 아직 인증이 없으니 뺐어요. 외부에 노출하는 시점에 API 키를 헤더로 붙이는 `Interceptor`를 `AddGrpcClient(...).AddInterceptor<...>()`로 추가하면 되고, 이때 서버가 키별로 허용 페르소나를 검증하게 이어 가면 돼요.

다음은 서버의 `ChatServiceImpl`에 Ollama 호출을 붙일 차례일 텐데, 필요하시면 `Personas` 설정을 읽어서 `StreamChat`을 구현하는 초안도 이어서 잡아볼게요.
