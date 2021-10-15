using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public interface IOperation<in T> : ISelect<T, ValueTask> {}

public interface IOperation : IResult<ValueTask> {}