using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public readonly record struct Worker(Task Monitor, ICompleted Complete) : IDisposable
{
	public void Dispose()
	{
		Complete.Dispose();
	}
}