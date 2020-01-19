using System.Threading.Tasks;

namespace DragonSpark.Compose.Extents.Selections {
	public delegate TOut Parameter<TIn, out TOut>((TIn Parameter, ValueTask Task) context);
}