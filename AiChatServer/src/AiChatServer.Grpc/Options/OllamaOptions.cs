using System.ComponentModel.DataAnnotations;

namespace AiChatServer.Grpc.Options;

public sealed class OllamaOptions
{
    public const string Section = "Ollama";

    [Required, Url]
    public string BaseUrl { get; set; } = string.Empty;

    [Range(10, 1800)]
    public int RequestTimeoutSeconds { get; set; } = 300;
}
