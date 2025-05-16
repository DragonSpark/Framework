using System.Threading.Tasks;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Allocated;

public interface IAllocatingToken<T, TOut> : ISelect<Token<T>, Task<TOut>>; // TODO: -> StoppingAllocating

public interface IAllocatingToken<T> : ISelect<Token<T>, Task>;