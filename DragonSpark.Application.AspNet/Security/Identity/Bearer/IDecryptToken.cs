using System.Collections.Generic;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public interface IDecryptToken : IStopAware<string, IDictionary<string, object>?>;