using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Application.Hosting.Server.Testing
{
	[ApiController, Route("[controller]")]
	public sealed class HelloWorldController : ControllerBase
	{
		public string Get() => "Hello World!";
	}
}