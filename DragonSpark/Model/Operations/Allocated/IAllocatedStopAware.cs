using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public interface IAllocatedStopAware<T, TOut> : ISelect<Stop<T>, Task<TOut>>;

public interface IAllocatedStopAware<T> : ISelect<Stop<T>, Task>;