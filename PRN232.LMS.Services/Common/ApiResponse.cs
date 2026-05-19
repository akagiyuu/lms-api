namespace PRN232.LMS.Services.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public object? Errors { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Request processed successfully")
        => new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> Err(string message, object? errors = null)
        => new() { Success = fall, Message = message, Errors = errors };
}
