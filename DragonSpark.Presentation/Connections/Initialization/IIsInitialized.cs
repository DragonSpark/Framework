using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Initialization;

public interface IIsInitialized : ICondition<HttpContext> {}