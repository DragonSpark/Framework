using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations
{
	delegate ValueTask<TOut> Get<TIn, TOut>((TIn Parameter, object Key) parameter);
}