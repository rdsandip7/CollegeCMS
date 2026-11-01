namespace BusinessLayer;

/// <summary>Outcome of a write that can be refused for a business reason (not found, duplicate, in use).</summary>
public record OperationResult(bool Succeeded, string? Error = null)
{
    public static OperationResult Success { get; } = new(true);
    public static OperationResult Fail(string error) => new(false, error);
}
