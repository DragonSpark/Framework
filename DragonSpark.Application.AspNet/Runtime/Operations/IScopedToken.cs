using System.Threading;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Runtime.Operations;

public interface IScopedToken : IResult<CancellationToken>;
