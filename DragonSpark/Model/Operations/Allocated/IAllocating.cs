using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public interface IAllocating<in T, TOut> : ISelect<T, Task<TOut>> {}