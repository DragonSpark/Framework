using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections
{
	public interface IReceive : ISelect<Func<Task>, IReceiver> {}

	public interface IReceive<out T> : ISelect<Func<T, Task>, IReceiver> {}
}