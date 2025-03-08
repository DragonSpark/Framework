namespace DragonSpark.Presentation.Model;

public class SelectionListing : SelectionListing<string>;

public class SelectionListing<T>
{
	public T Value { get; set; } = default!;

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;

	public string? Tag { get; set; }

	public string Icon { get; set; } = null!;
}