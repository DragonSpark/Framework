using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

public interface IResulting<T> : IResult<ValueTask<T>>;