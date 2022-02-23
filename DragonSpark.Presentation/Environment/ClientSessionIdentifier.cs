using DragonSpark.Application.Components;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Presentation.Connections.Initialization;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class ClientSessionIdentifier : IFormatter<HttpContext>
{
	readonly ICondition<HttpContext>      _initialized;
	readonly ISelect<HttpContext, string> _true;
	readonly IClientIdentifier            _identifier;

	public ClientSessionIdentifier(IClientIdentifier identifier)
		: this(IsInitialized.Default, ClientIdentifierAccessor.Default.Then().Select(x => x.Value().ToString()).Get(),
		       identifier) {}

	public ClientSessionIdentifier(ICondition<HttpContext> initialized, ISelect<HttpContext, string> @true,
	                               IClientIdentifier identifier)
	{
		_initialized = initialized;
		_true        = @true;
		_identifier  = identifier;
	}

	public string Get(HttpContext parameter)
		=> _initialized.Get(parameter) ? _true.Get(parameter) : _identifier.Get().ToString();
}