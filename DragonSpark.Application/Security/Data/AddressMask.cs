using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;
using System.Linq;
using System.Net.Mail;

namespace DragonSpark.Application.Security.Data
{
	public sealed class AddressMask : IFormatter<MailAddress>
	{
		public static AddressMask Default { get; } = new AddressMask();

		AddressMask() : this(Mask.Default) {}

		readonly IAlteration<string> _mask;

		public AddressMask(IAlteration<string> mask) => _mask = mask;

		public string Get(MailAddress parameter)
		{
			var parts   = parameter.Host.Split('.');
			var domain  = _mask.Get(string.Join('.', parts[..^1]));
			var result  = $"{_mask.Get(parameter.User)}@{domain}.{parts.Last()}";
			return result;
		}
	}
}