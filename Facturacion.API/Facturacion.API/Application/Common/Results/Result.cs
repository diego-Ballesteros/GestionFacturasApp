namespace Facturacion.API.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; } 

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new Result<TValue>(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public TValue Value => IsSuccess
        ? _value! 
        : throw new InvalidOperationException("Cannot access value of a failed result.");

    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }
}

public record Error(string Code, string Message) 
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "A null value was provided.");
    
    public static Error NotFound(string resourceName, object resourceIdentifier) =>
    new("Error.NotFound", $"The resource '{resourceName}' with identifier '{resourceIdentifier}' was not found.");
}

