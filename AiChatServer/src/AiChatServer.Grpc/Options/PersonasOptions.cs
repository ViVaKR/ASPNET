using System.ComponentModel.DataAnnotations;
using AiChatServer.Contracts.V1;

namespace AiChatServer.Grpc.Options;

public class PersonaOptions
{
    [Required]
    public string Model { get; set; } = string.Empty;

    public bool Think { get; set; }

    public string? SystemPrompt { get; set; }   // null이면 Modelfile의 SYSTEM 사용
}

public sealed class PersonasOptions : Dictionary<Persona, PersonaOptions>
{
    public const string Section = "Personas";
}
