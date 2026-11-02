using AiChatServer.Contracts.V1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AiChat.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAiChatClient(
        this IServiceCollection services,
        Action<AiChatClientOptions> configure)
    {
        services.AddOptions<AiChatClientOptions>()
            .Configure(configure)
            .ValidateDataAnnotations()
            .Validate(o => o.Persona != Persona.Unspecified, "AiChat: Persona 를 지정해야 합니다.")
            .ValidateOnStart(); // 잘못되면 시작 단계에서 즉시 실패 → K8s CrashLoop 로 바로 드러남

        services.AddGrpcClient<ChatService.ChatServiceClient>((sp, o) =>
            {
                o.Address = new Uri(sp.GetRequiredService<IOptions<AiChatClientOptions>>().Value.Address);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                // 오래 걸리는 스트림 중 유휴 연결이 끊기지 않도록 keep-alive
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                EnableMultipleHttp2Connections = true
            });

        services.TryAddTransient<IAiChatClient, GrpcAiChatClient>();
        return services;
    }

    /// <summary>appsettings 의 섹션에서 읽는 버전. 예) "AiChat": { "Address": "...", "Persona": "Beopdeung" }</summary>
    public static IServiceCollection AddAiChatClient(
        this IServiceCollection services,
        Microsoft.Extensions.Configuration.IConfiguration section)
        => services.AddAiChatClient(o => section.Bind(o));
}
