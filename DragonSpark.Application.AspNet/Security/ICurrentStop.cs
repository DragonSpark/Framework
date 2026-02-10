using System.Threading;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Security;

public interface ICurrentStop : IResult<CancellationToken>;