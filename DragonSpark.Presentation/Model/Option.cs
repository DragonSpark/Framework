namespace DragonSpark.Presentation.Model;

public class Option : Option<string>;

public class Option<T>
{
	public T Value { get; set; } = default!;

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;

	public string? Tag { get; set; }

	public string Icon { get; set; } = null!;
}