using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Initialization
{
	public interface IInitializeConnection : ICommand<HttpContext> {}
}