namespace HumanFrogApi.Services;

public enum ServiceErrorType
{
    NotFound,
    Validation,
    Conflict
}

public record ServiceError(ServiceErrorType Type, string Message);

public class ServiceResult<T>
{
    public T? Value { get; }
    public ServiceError? Error { get; }
    public bool IsSuccess => Error is null;

    private ServiceResult(T? value, ServiceError? error)
    {
        Value = value;
        Error = error;
    }

    public static ServiceResult<T> Success(T value) => new(value, null);
    public static ServiceResult<T> NotFound(string message) => new(default, new ServiceError(ServiceErrorType.NotFound, message));
    public static ServiceResult<T> Validation(string message) => new(default, new ServiceError(ServiceErrorType.Validation, message));
    public static ServiceResult<T> Conflict(string message) => new(default, new ServiceError(ServiceErrorType.Conflict, message));
}
