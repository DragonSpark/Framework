using DragonSpark.Model.Selection;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class CreateFilterBody<T> : ISelect<string, IQuery<T>>
{
	public static CreateFilterBody<T> Default { get; } = new();

	CreateFilterBody() {}

	public IQuery<T> Get(string parameter)
		=> new BodyQuery<T>(Search<T>.Default, new FilterField<T>(parameter), Where<T>.Default, Sort<T>.Default,
		                    Filter<T>.Default);
}