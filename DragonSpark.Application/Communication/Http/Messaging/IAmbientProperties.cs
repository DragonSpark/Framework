using System.Collections.Generic;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http.Messaging;

public interface IAmbientProperties : IResult<IEnumerable<KeyValuePair<string, string>>>;