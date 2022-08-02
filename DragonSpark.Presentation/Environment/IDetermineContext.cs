using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

public interface IDetermineContext : IAlteration<HttpContext> {}