using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Messaging;

public class MessageTemplate<T> : IMessageTemplate<T>
{
	readonly Func<T, string> _to, _title, _template;

	protected MessageTemplate(string address, string title, ISelect<T, string> template)
		: this(address.Accept, title, template) {}

	protected MessageTemplate(Func<T, string> to, string title, ISelect<T, string> template)
		: this(to, title, template.Get) {}

	protected MessageTemplate(Func<T, string> to, string title, Func<T, string> template)
		: this(to, title.Accept, template) {}

	protected MessageTemplate(Func<T, string> to, Func<T, string> title, Func<T, string> template)
	{
		_to       = to;
		_title    = title;
		_template = template;
	}

	public ValueTask<Message> Get(T parameter)
		=> new Message(_to(parameter), _title(parameter), _template(parameter)).ToOperation();
}