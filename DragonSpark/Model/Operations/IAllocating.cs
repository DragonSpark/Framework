using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public interface IAllocating<in T, TOut> : ISelect<T, Task<TOut>> {}