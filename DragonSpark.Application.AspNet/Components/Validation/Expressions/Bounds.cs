namespace DragonSpark.Application.Components.Validation.Expressions;

public readonly struct Bounds
{
	public Bounds(uint minimum, uint maximum)
	{
		Minimum = minimum;
		Maximum = maximum;
	}

	public uint Minimum { get; }

	public uint Maximum { get; }
}