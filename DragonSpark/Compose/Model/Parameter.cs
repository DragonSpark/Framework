using System.Threading.Tasks;

namespace DragonSpark.Compose.Model
{
	public delegate TOut Parameter<TIn, out TOut>((TIn Parameter, ValueTask Task) context);
}