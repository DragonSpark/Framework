using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

public interface IInitializeContext : ICommand<HttpContext> {}