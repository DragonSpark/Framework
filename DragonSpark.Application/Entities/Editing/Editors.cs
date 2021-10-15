using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public sealed class Editors : Editors<None>
{
	public Editors(IEnlistedScopes scopes) : base(scopes) {}
}

public class Editors<T> : ISelecting<T, IEditor>
{
	readonly IScopes _scopes;

	public Editors(IScopes scopes) => _scopes = scopes;

	public async ValueTask<IEditor> Get(T parameter)
	{
		var (context, disposable) = _scopes.Get();
		return new Editor(context, await disposable.Await());
	}
}