using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Presentation.Environment;

sealed class ReferrerAwareEstablishContext : IEstablishContext
{
	readonly IEstablishContext            _previous;
	readonly ISelect<HttpRequest, string?> _header;
	readonly Message                       _message;

	public ReferrerAwareEstablishContext(IEstablishContext previous, Message message)
		: this(previous, HostAwareReferrerHeader.Default, message) {}

	public ReferrerAwareEstablishContext(IEstablishContext previous, ISelect<HttpRequest, string?> header,
	                                      Message message)
	{
		_previous = previous;
		_header   = header;
		_message  = message;
	}

	public void Execute(HttpContext parameter)
	{
		_previous.Execute(parameter);
		var referrer = _header.Get(parameter.Request);
		if (referrer is not null)
		{
			_message.Execute(referrer);
		}
	}

	internal sealed class Message : LogMessage<string>
	{
		public Message(ILogger<Message> logger) : base(logger, "This request has a referrer: {Referrer}") {}
	}
}