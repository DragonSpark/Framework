using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public sealed class Editors : Editors<None>
{
	public Editors(IEnlistedScopes scopes) : base(scopes) {}
}

public class Editors<T> : ISelect<Stop<T>, IEditor>
{
	readonly IScopes _scopes;

	protected Editors(IScopes scopes) => _scopes = scopes;

	[MustDisposeResource]
	public IEditor Get(Stop<T> parameter)
	{
		var (context, disposable) = _scopes.Get();
		return new Editor(context, disposable, parameter);
	}
}
