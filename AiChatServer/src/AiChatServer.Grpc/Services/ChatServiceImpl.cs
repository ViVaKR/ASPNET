using AiChatServer.Contracts.V1;
using Grpc.Core;

namespace AiChatServer.Grpc.Services;

public sealed class ChatServiceImpl(ILogger<ChatServiceImpl> logger)
    : ChatService.ChatServiceBase
{
    /// <summary>
    /// Complet
    /// rpc Complete (ChatRequest) returns (ChatReply);
    /// </summary>
    public override async Task<ChatReply> Complete(
        ChatRequest request,
        ServerCallContext context)
    {
        logger.LogInformation("=== Start ===");
        return await Task.FromResult(new ChatReply
        {
            ConversationId = request.ConversationId,
            Message = new ChatMessage
            {
                Role = ChatRole.Assistant,
                Content = "도반님, 안녕하십니까."
            }
        });
    }

    /// <summary>
    /// StreamChat
    /// rpc StreamChat (ChatRequest) returns (stream ChatChunk);
    /// </summary>
    public override async Task StreamChat(
        ChatRequest request,
        IServerStreamWriter<ChatChunk> stream,
        ServerCallContext context)
    {
        var ct = context.CancellationToken;

        foreach (var word in new[] { "도반님, ", "안녕하십니까!", "법등입니다." })
        {
            await stream.WriteAsync(new ChatChunk
            {
                ConversationId = request.ConversationId,
                Kind = ChunkKind.Content,
                Delta = word
            }, ct);

            await Task.Delay(200, ct);
        }

        await stream.WriteAsync(new ChatChunk
        {
            ConversationId = request.ConversationId,
            Kind = ChunkKind.Content,
            Done = true
        }, ct);
    }
}
