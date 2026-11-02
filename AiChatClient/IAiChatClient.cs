namespace AiChat.Client;

public interface IAiChatClient
{
    /// <summary>한 번에 전체 답변을 받는다 (rpc Complete).</summary>
    Task<AiReply> CompleteAsync(
        Guid conversationId,
        string question,
        IReadOnlyList<AiTurn> history,
        CancellationToken ct = default);

    /// <summary>토큰 단위로 답변을 받는다 (rpc StreamChat).</summary>
    IAsyncEnumerable<AiChunk> StreamAsync(
        Guid conversationId,
        string question,
        IReadOnlyList<AiTurn> history,
        CancellationToken ct = default);
}
