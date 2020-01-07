using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Server.GitHub.Testing.Application.Controllers
{
	[ApiController, Route("[controller]")]
	public sealed class EventsController : ControllerBase
	{
		readonly IOperation<EventMessage> _operation;

		public EventsController(LoggedProcessorOperation<EventsController> operation) : this(operation.Get()) {}

		EventsController(IOperation<EventMessage> operation) => _operation = operation;

		[HttpPost]
		public ValueTask Post(EventMessage message) => _operation.Get(message);

		[HttpGet]
		public string Get() => "Hello World!";
	}

	sealed class CustomHandler : IHandler<IssueCommentPayload>
	{
		/*readonly GitHubClient _client;

		public CustomHandler(GitHubClient client) => _client = client;*/

		public ValueTask Get(IssueCommentPayload parameter) => new ValueTask(Task.CompletedTask);
	}
}