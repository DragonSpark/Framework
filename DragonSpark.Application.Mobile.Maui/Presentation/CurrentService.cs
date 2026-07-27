using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public sealed class CurrentService<T> : Result<T> where T : notnull
{
    public static CurrentService<T> Default { get; } = new();

    CurrentService() : base(CurrentServices.Default.GetRequiredService<T>) {}
}