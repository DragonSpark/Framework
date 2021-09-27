using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	internal class Class1 {}

	public readonly record struct In<T>(DbContext Context, T Parameter);

	sealed class Contextual<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly IScopes             _scopes;
		readonly IContext<TIn, TOut> _context;

		public Contextual(IScopes scopes, IContext<TIn, TOut> context)
		{
			_scopes  = scopes;
			_context = context;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			var (subject, boundary) = _scopes.Get();
			using var _      = await boundary.Await();
			var       result = await _context.Await(new In<TIn>(subject, parameter));
			return result;
		}
	}

	/*public interface IContexts : IResult<DbContext> {}*/

	public interface IContext<TIn, T> : ISelecting<In<TIn>, T> {}
}