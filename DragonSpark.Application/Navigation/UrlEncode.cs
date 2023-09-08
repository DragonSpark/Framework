using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;

namespace DragonSpark.Application.Navigation;

public sealed class UrlEncode : Alteration<string>
{
	public static UrlEncode Default { get; } = new();

	UrlEncode() : base(WebUtility.UrlEncode) {}
}

public sealed class UrlDecode : Alteration<string>
{
	public static UrlDecode Default { get; } = new();

	UrlDecode() : base(WebUtility.UrlDecode) {}
}

public sealed class Base64UrlEncode : Alteration<string>
{
	public static Base64UrlEncode Default { get; } = new();

	Base64UrlEncode() : base(EncodedTextAsData.Default.Then().Subject.Select(WebEncoders.Base64UrlEncode)) {}
}
// TODO
public sealed class Base64UrlDecode : Alteration<string>
{
	public static Base64UrlDecode Default { get; } = new();

	Base64UrlDecode() : this(Encoding.UTF8) {}

	public Base64UrlDecode(Encoding encoding)
		: base(Start.A.Selection<string, byte[]>(WebEncoders.Base64UrlDecode).Select(encoding.GetString)) {}
}