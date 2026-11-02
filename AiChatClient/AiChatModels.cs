namespace AiChat.Client;

// 사이트(Chat 서버) 코드가 proto 생성 타입을 직접 만지지 않도록 하는 경계 타입들.
// proto 가 바뀌어도 이 파일과 GrpcAiChatClient 만 고치면 된다.

public enum AiRole { User, Assistant }

public sealed record AiTurn(AiRole Role, string Content);

public enum AiChunkKind { Content, Thinking }

public sealed record AiUsage(int PromptTokens, int CompletionTokens);

/// <summary>스트리밍 조각. Done=true 인 마지막 조각에만 Usage 가 채워질 수 있다.</summary>
public readonly record struct AiChunk(
    string Delta,
    AiChunkKind Kind,
    bool Done = false,
    AiUsage? Usage = null);

public sealed record AiReply(string Content, AiUsage? Usage);
