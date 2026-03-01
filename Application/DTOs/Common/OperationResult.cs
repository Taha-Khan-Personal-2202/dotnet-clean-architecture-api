public sealed class OperationResult<T>
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public int StatusCode { get; init; }
    public T? Data { get; init; }

    public static OperationResult<T> Ok(T? data, string message = "Success", int statusCode = 200)
        => new()
        {
            Success = true,
            Message = message,
            StatusCode = statusCode,
            Data = data
        };

    public static OperationResult<T> Error(string message, int statusCode = 400, T? data = default)
        => new()
        {
            Success = false,
            Message = message,
            StatusCode = statusCode,
            Data = data
        };
}