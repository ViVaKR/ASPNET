using System.Runtime.CompilerServices;
using AiChatServer.Contracts.V1;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace AiChat.Client;

public sealed class GrpcAiChatClient(
    ChatService.ChatServiceClient grpc,
    IOptions<AiChatClientOptions> options) : IAiChatClient
{
    private readonly AiChatClientOptions _o = options.Value;

    public async Task<AiReply> CompleteAsync(
        Guid conversationId, string question, IReadOnlyList<AiTurn> history, CancellationToken ct = default)
    {
        var request = BuildRequest(conversationId, question, history);

        var reply = await grpc.CompleteAsync(
            request,
            deadline: DateTime.UtcNow.AddSeconds(_o.CompleteTimeoutSeconds),
            cancellationToken: ct);

        return new AiReply(reply.Message?.Content ?? string.Empty, MapUsage(reply.Usage));
    }

    public async IAsyncEnumerable<AiChunk> StreamAsync(
        Guid conversationId, string question, IReadOnlyList<AiTurn> history,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var request = BuildRequest(conversationId, question, history);

        using var call = grpc.StreamChat(
            request,
            deadline: DateTime.UtcNow.AddSeconds(_o.StreamTimeoutSeconds),
            cancellationToken: ct);

        await foreach (var c in call.ResponseStream.ReadAllAsync(ct))
        {
            yield return new AiChunk(
                c.Delta,
                c.Kind == ChunkKind.Thinking ? AiChunkKind.Thinking : AiChunkKind.Content,
                c.Done,
                MapUsage(c.Usage));
        }
    }

    // ---- 변환 -------------------------------------------------------------

    private ChatRequest BuildRequest(Guid conversationId, string question, IReadOnlyList<AiTurn> history)
    {
        if (string.IsNullOrWhiteSpace(question))
            throw new ArgumentException("질문이 비어 있습니다.", nameof(question));

        var request = new ChatRequest
        {
            ConversationId = conversationId.ToString(),
            Persona = _o.Persona
        };

        foreach (var t in history.TakeLast(_o.MaxHistory))
        {
            request.Messages.Add(new ChatMessage
            {
                Role = t.Role == AiRole.User ? ChatRole.User : ChatRole.Assistant,
                Content = t.Content
            });
        }

        // 이번 질문은 항상 마지막 user 메시지
        request.Messages.Add(new ChatMessage { Role = ChatRole.User, Content = question });
        return request;
    }

    private static AiUsage? MapUsage(Usage? u)
        => u is null ? null : new AiUsage(u.PromptTokens, u.CompletionTokens);
}
