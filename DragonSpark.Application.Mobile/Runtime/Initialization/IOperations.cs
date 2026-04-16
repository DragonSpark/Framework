using System.Collections.Generic;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public interface IOperations<T> : IResult<List<T>>, IStopAware;