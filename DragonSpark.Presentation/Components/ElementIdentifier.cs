namespace DragonSpark.Presentation.Components;

public readonly record struct ElementIdentifier(string Name)
{
	public static implicit operator ElementIdentifier(string instance) => new(instance);

	public override string ToString() => $"#{Name}";
}