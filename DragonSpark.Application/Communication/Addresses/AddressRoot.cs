using DragonSpark.Model.Selection.Alterations;
using System.Net.Mail;

namespace DragonSpark.Application.Communication.Addresses;

public sealed class AddressRoot : IAlteration<string>
{
	public static AddressRoot Default { get; } = new();

	AddressRoot() {}

	public string Get(string parameter)
	{
		var address = new MailAddress(parameter);
		var result  = $"{address.User.Split('+')[0]}@{address.Host}";
		return result;
	}
}