using DragonSpark.Application.Communication;
using DragonSpark.Diagnostics.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Presentation.Environment;

sealed class ReferrerAwareInitializeContext : IInitializeContext
{
	readonly IInitializeContext _previous;
	readonly IHeader            _header;
	readonly Message            _message;

	public ReferrerAwareInitializeContext(IInitializeContext previous, Message message)
		: this(previous, RefererHeader.Default, message) {}

	public ReferrerAwareInitializeContext(IInitializeContext previous, IHeader header, Message message)
	{
		_previous = previous;
		_header   = header;
		_message  = message;
	}

	public void Execute(HttpContext parameter)
	{
		_previous.Execute(parameter);
		var referrer = _header.Get(parameter.Request.Headers);
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