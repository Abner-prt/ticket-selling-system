namespace ticket_selling_backend.DTOs;

public class ResponseDto<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Status { get; set; }
    public T? Data { get; set; }

    public static ResponseDto<T> Success(T data, string message = "Success", int statusCode = 200)
    {
        return new ResponseDto<T> { Status = true, StatusCode = statusCode, Message = message, Data = data };
    }

    public static ResponseDto<T> Failure(string message, int statusCode = 400)
    {
        return new ResponseDto<T> { Status = false, StatusCode = statusCode, Message = message };
    }
}
