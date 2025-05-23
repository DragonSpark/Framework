using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Messaging;

public class SendMessage<T> : IOperation<T>
{
	readonly IEmailSender        _sender;
	readonly IMessageTemplate<T> _template;

	protected SendMessage(IEmailSender sender, IMessageTemplate<T> template)
	{
		_sender   = sender;
		_template = template;
	}

	public async ValueTask Get(T parameter)
	{
		var (to, title, body) = await _template.Off(parameter);

		await _sender.SendEmailAsync(to, title, body).Off();
	}
}