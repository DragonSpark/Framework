using DragonSpark.Application.Security.Data;
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

// TODO

public sealed class Base64UrlDecrypt : Alteration<string>
{
	public Base64UrlDecrypt(IDecryptText select) : base(Base64UrlDecode.Default.Then().Select(select)) {}
}

public sealed class UrlDecrypt : Alteration<string>
{
	public UrlDecrypt(IDecryptText select) : base(UrlDecode.Default.Then().Select(select)) {}
}