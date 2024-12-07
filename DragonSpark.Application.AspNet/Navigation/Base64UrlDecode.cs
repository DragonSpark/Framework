using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace DragonSpark.Application.Navigation;

public sealed class Base64UrlDecode : Alteration<string>
{
	public static Base64UrlDecode Default { get; } = new();

	Base64UrlDecode() : this(Encoding.UTF8) {}

	public Base64UrlDecode(Encoding encoding)
		: base(Start.A.Selection<string, byte[]>(WebEncoders.Base64UrlDecode).Select(encoding.GetString)) {}
}