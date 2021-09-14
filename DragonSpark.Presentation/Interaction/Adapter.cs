using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Interaction
{
	sealed class Adapter<T> : IInteractionResultHandler where T : IInteractionResult
	{
		readonly IOperation<T> _operation;

		public Adapter(IOperation<T> operation) => _operation = operation;

		public ValueTask Get(IInteractionResult parameter) => _operation.Get(parameter.To<T>());
	}
}