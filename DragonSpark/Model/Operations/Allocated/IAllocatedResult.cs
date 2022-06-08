using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public interface IAllocatedResult<T> : IResult<Task<T>> {}