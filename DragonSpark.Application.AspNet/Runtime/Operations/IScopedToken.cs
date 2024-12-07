using DragonSpark.Model.Results;
using System.Threading;

namespace DragonSpark.Application.Runtime.Operations;

public interface IScopedToken : IResult<CancellationToken>;