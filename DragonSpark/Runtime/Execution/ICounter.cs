using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;

namespace DragonSpark.Runtime.Execution;

public interface ICounter : IResult<int>, ICommand {}