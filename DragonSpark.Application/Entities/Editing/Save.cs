using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public sealed class Save : IOperation
{
	readonly IEnlistedScopes _scopes;

	public Save(IEnlistedScopes scopes) => _scopes = scopes;

	public async ValueTask Get()
	{
		var       scope = _scopes.Get();
		using var _     = await scope.Boundary.Await();
		await scope.Subject.SaveChangesAsync().ConfigureAwait(false);
	}
}

public class Save<T> : Update<T> where T : class
{
	public Save(IEnlistedScopes scopes) : base(scopes) {}

	protected Save(IScopes scopes) : base(scopes) {}
}