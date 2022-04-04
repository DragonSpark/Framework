using DragonSpark.Application.Diagnostics.Time;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Runtime;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class DetermineSecurityDescriptor : ISelect<ClaimsIdentity, SecurityTokenDescriptor>
{
	readonly ITable<ClaimsIdentity, SecurityTokenDescriptor> _source;
	readonly IWindow                                         _window;

	public DetermineSecurityDescriptor(SecurityDescriptor descriptor, BearerSettings settings)
		: this(descriptor.Then().Stores().New(), Time.Default.FromNow(settings.Window)) {}

	public DetermineSecurityDescriptor(ITable<ClaimsIdentity, SecurityTokenDescriptor> source, IWindow window)
	{
		_source = source;
		_window = window;
	}

	public SecurityTokenDescriptor Get(ClaimsIdentity parameter)
	{
		var stored = _source.Get(parameter);
		var expired = stored.Expires.HasValue
		              &&
		              _window.Get(stored.Expires.GetValueOrDefault())
		              &&
		              _source.Remove(parameter);
		var result = expired ? _source.Get(parameter) : stored;
		return result;
	}
}