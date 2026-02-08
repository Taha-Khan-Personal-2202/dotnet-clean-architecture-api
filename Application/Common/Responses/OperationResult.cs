namespace Application.Common.Responses;

public class OperationResult<T>
{
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public int StatusCode { get; private set; }
    public T? Data { get; private set; }

    public static OperationResult<T> Success(T? data, string message = "Request completed successfully.", int statusCode = 200)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data,
            StatusCode = statusCode
        };
    }

    public static OperationResult<T> Fail(string message, int statusCode = 400, T? data = default)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            Message = message,
            Data = data,
            StatusCode = statusCode
        };
    }
}
