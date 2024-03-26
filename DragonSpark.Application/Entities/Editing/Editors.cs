using DragonSpark.Model;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Editing;

public sealed class Editors : Editors<None>
{
	public Editors(IEnlistedScopes scopes) : base(scopes) {}
}

public class Editors<T> : ISelect<T, IEditor>
{
	readonly IScopes _scopes;

	protected Editors(IScopes scopes) => _scopes = scopes;

	public IEditor Get(T parameter)
	{
		var (context, disposable) = _scopes.Get();
		return new Editor(context, disposable);
	}
}