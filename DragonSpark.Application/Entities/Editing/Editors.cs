using DragonSpark.Model;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Editing;

public sealed class Editors : Editors<None>
{
	public Editors(IEnlistedContexts contexts) : base(contexts) {}
}

public class Editors<T> : ISelect<T, IEditor>
{
	readonly IContexts _contexts;

	protected Editors(IContexts contexts) => _contexts = contexts;

	public IEditor Get(T parameter)
	{
		var (context, disposable) = _contexts.Get();
		return new Editor(context, disposable);
	}
}