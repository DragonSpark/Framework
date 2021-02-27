using DragonSpark.Model.Selection.Alterations;
using System.Linq;
using System.Net.Mail;

namespace DragonSpark.Application.Security.Data
{
	public sealed class AddressMask : IAlteration<string>
	{
		public static AddressMask Default { get; } = new AddressMask();

		AddressMask() : this(Mask.Default) {}

		readonly IAlteration<string> _mask;

		public AddressMask(IAlteration<string> mask) => _mask = mask;

		public string Get(string parameter)
		{
			var address = new MailAddress(parameter);
			var parts   = address.Host.Split('.');
			var suffix  = parts.Last();
			var domain  = _mask.Get(string.Join('.', parts[..^1]));
			var result  = $"{_mask.Get(address.User)}@{domain}.{suffix}";
			return result;
		}
	}
}