using Bogus;

namespace DragonSpark.Application.Compose.Entities.Generation;

public readonly struct PivotGenerators<T, TPivot, TCurrent>
	where T : class where TPivot : class where TCurrent : class
{
	public PivotGenerators(Faker<T> subject, Faker<TPivot> pivot, Faker<TCurrent> current)
	{
		Subject = subject;
		Pivot   = pivot;
		Current = current;
	}

	public Faker<T> Subject { get; }

	public Faker<TPivot> Pivot { get; }

	public Faker<TCurrent> Current { get; }

	public void Deconstruct(out Faker<T> subject, out Faker<TPivot> pivot, out Faker<TCurrent> current)
	{
		subject = Subject;
		pivot   = Pivot;
		current = Current;
	}
}