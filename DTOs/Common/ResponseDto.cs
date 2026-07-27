using System.Text.Json.Serialization;

namespace ticket_selling_backend.Dtos.Common;

public class ResponseDto<T>
{
    [JsonIgnore]
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Status { get; set; }
    public T? Data { get; set; }
}
