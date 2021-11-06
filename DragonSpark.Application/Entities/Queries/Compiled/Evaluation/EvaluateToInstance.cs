using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToInstance<T> : EvaluateToInstance<None, T>, IResulting<T>
{
	public EvaluateToInstance(IScopes scopes, IInstance<None, T> instance) : base(scopes, instance) {}

	public EvaluateToInstance(IScopes scopes, IElement<None, T> element) : base(scopes, element) {}

	public ValueTask<T> Get() => Get(None.Default);
}

public class EvaluateToInstance<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly IScopes             _scopes;
	readonly IElement<TIn, TOut> _element;

	public EvaluateToInstance(IScopes scopes, IInstance<TIn, TOut> instance)
		: this(scopes, new Element<TIn, TOut>(instance)) {}

	public EvaluateToInstance(IScopes scopes, IElement<TIn, TOut> element)
	{
		_scopes  = scopes;
		_element = element;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var (context, boundary) = _scopes.Get();
		using var start  = await boundary.Await();
		var       result = await _element.Await(new(context, parameter));
		return result;
	}
}