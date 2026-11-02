using AiChatServer.Grpc.Options;
using AiChatServer.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// 설정: 잘못되면 시작 단계에서 즉시 실패 (K8s에서 CrashLoop로 바로 드러남)
builder.Services.AddOptions<OllamaOptions>()
    .BindConfiguration(OllamaOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<PersonasOptions>()
    .BindConfiguration(PersonasOptions.Section)
    .Validate(p => p.Count > 0, "Personas 설정이 비어 있습니다.")
    .ValidateOnStart();

builder.Services.AddGrpc(x =>
{
    x.MaxReceiveMessageSize = 1 * 1024 * 1024; // 요청 1MB 상한
    x.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

builder.Services.AddGrpcReflection();
builder.Services.AddGrpcHealthChecks(); // grpc.health.v1
builder.Services.AddHealthChecks(); // HTTP /healthz
builder.Services.Configure<HostOptions>(x => x.ShutdownTimeout = TimeSpan.FromSeconds(30));

var app = builder.Build();

app.MapGrpcService<ChatServiceImpl>();
app.MapGrpcHealthChecksService(); // K8s grpc probe용 (8080)
app.MapHealthChecks("/healthz/live").RequireHost("*:8081");
app.MapHealthChecks("/healthz/ready").RequireHost("*:8081");

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGet("/demo", () => "Fine Thanks And You?");
app.Run();
