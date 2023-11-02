using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

public sealed class ContextStore : Variable<HttpContext?>;