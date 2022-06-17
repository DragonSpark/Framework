using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections;

public interface IInitializeConnection : ICommand<HttpContext> {}