using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public interface IAllocatedResult<T> : IResult<Task<T>> {}