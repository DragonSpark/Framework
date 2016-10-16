using System;

namespace DragonSpark.Commands
{
	public interface IExecute
	{
		void Execute();
	}

	public interface IExecution : IExecute, IDisposable {}
}