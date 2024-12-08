using DragonSpark.Application.AspNet.Navigation;
using DragonSpark.Application.AspNet.Navigation.Security;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using Flurl;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public sealed class DeviceInterceptionAwareRemoteFailure : IAllocated<RemoteFailureContext>
{
	public static DeviceInterceptionAwareRemoteFailure Default { get; } = new();

	DeviceInterceptionAwareRemoteFailure()
		: this(RemoteFailure.Default, AuthenticateAddress.Default, ExtractReturnAddress.Default) {}

	readonly IAllocated<RemoteFailureContext>     _previous;
	readonly IFormatter<AuthenticateAddressInput> _address;
	readonly ISelect<string, string?>             _extract;

	public DeviceInterceptionAwareRemoteFailure(IAllocated<RemoteFailureContext> previous,
	                                            IFormatter<AuthenticateAddressInput> address,
	                                            ISelect<string, string?> extract)
	{
		_previous = previous;
		_address  = address;
		_extract  = extract;
	}

	public Task Get(RemoteFailureContext parameter)
	{
		var failure = parameter.Failure;
		if (failure is not null)
		{
			var logger = parameter.HttpContext.RequestServices.GetRequiredService<ILogger<RemoteFailure>>();
			if (failure.Message == "Correlation failed.")
			{
				var uri = parameter.Properties?.RedirectUri;
				if (uri is not null && !uri.Contains("InterceptionAware=1"))
				{
					var @return = _address.Get(new(parameter.Scheme.Name, _extract.Get(uri) ?? "/"));
					var address = Url.Parse(@return).SetQueryParam("InterceptionAware", "1");
					parameter.Response.Redirect(address);
					parameter.HandleResponse();
					return Task.CompletedTask;
				}

				logger.LogWarning(failure, "An iOS native application Safari instance intercepted the login workflow");
			}
			else
			{
				logger.LogError(failure, "There was a problem handing a remote authentication");
			}
		}

		return _previous.Get(parameter);
	}
}