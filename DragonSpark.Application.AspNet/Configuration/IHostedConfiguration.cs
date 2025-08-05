using System.Collections.Generic;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Configuration;

public interface IHostedConfiguration : IResult<IReadOnlyDictionary<string, object?>>;