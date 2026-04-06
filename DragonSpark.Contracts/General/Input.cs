namespace DragonSpark.Contracts.General;

public sealed class Input(string value)
{
    public string Value { get; } = value;
}