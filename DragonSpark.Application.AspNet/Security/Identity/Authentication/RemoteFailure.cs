using DragonSpark.Application.AspNet.Navigation.Security;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Text;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public sealed class RemoteFailure : IAllocated<RemoteFailureContext>
{
	public static RemoteFailure Default { get; } = new();

	RemoteFailure() : this(LoginProblemPath.Default) {}

	readonly IFormatter<string> _problem;

	public RemoteFailure(IFormatter<string> problem) => _problem = problem;

	public Task Get(RemoteFailureContext parameter)
	{
		parameter.Response.Redirect(_problem.Get(parameter.Scheme.Name.ToLowerInvariant()));
		parameter.HandleResponse();
		return Task.CompletedTask;
	}
}