using System.Threading.Tasks;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Invocation.Operations
{
	public interface IObserve<T> : ISelect<Task<T>, T> {}
}