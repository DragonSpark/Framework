using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Presentation.Connections
{
	public interface IReceiver : IOperation, IAsyncDisposable {}
}