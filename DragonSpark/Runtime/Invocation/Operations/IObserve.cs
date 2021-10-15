using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Runtime.Invocation.Operations;

public interface IObserve<T> : ISelect<Task<T>, T> {}