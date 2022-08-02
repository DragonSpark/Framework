using DragonSpark.Application.Communication;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class HostAwareReferrerHeader : ISelect<HttpRequest, string?>
{
	public static HostAwareReferrerHeader Default { get; } = new();

	HostAwareReferrerHeader() : this(RefererHeader.Default, GetRequestHost.Default) {}

	readonly IHeader                 _header;
	readonly IFormatter<HttpRequest> _host;

	public HostAwareReferrerHeader(IHeader header, IFormatter<HttpRequest> host)
	{
		_header = header;
		_host   = host;
	}

	public string? Get(HttpRequest parameter)
	{
		var host     = _host.Get(parameter);
		var referrer = _header.Get(parameter.Headers);
		var result   = referrer?.Contains(host) ?? false ? null : referrer;
		return result;
	}
}