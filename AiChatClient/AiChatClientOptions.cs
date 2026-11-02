using System.ComponentModel.DataAnnotations;
using AiChatServer.Contracts.V1;

namespace AiChat.Client;

public sealed class AiChatClientOptions
{
    /// <summary>AI 서버 주소. 예) http://buddham-ai-svc:50051</summary>
    [Required, Url]
    public string Address { get; set; } = string.Empty;

    /// <summary>이 사이트가 사용할 페르소나. 요청마다 받지 않고 여기서 고정한다.</summary>
    public Persona Persona { get; set; } = Persona.Unspecified;

    /// <summary>Complete(단발) 호출의 마감 시간.</summary>
    [Range(5, 1800)]
    public int CompleteTimeoutSeconds { get; set; } = 120;

    /// <summary>StreamChat 전체 마감 시간. nginx 의 proxy_read_timeout(600s)과 맞춰 둔 값.</summary>
    [Range(5, 1800)]
    public int StreamTimeoutSeconds { get; set; } = 600;

    /// <summary>대화 히스토리 상한 (서버 보호 + 토큰 절약). 오래된 것부터 버린다.</summary>
    [Range(1, 200)]
    public int MaxHistory { get; set; } = 20;
}
