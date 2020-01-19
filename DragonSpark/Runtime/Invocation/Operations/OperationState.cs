using DragonSpark.Runtime.Execution;
using System.Threading;

namespace DragonSpark.Runtime.Invocation.Operations
{
	sealed class OperationState
	{
		public OperationState(string name, CancellationToken token = new CancellationToken())
			: this(new ContextDetails(name), token) {}

		public OperationState(ContextDetails contextDetails, CancellationToken token = new CancellationToken())
		{
			ContextDetails = contextDetails;
			Token          = token;
		}

		public ContextDetails ContextDetails { get; }

		public CancellationToken Token { get; }
	}
}