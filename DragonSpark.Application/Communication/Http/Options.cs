namespace DragonSpark.Application.Communication.Http;

public record Options(string? Address = null, bool Configure = true)
{
    public static Options Default { get; } = new();
}