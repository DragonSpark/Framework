using DragonSpark.Application.Security.Data;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace DragonSpark.Application.Navigation;

public sealed class Base64UrlEncode : Alteration<string>
{
	public static Base64UrlEncode Default { get; } = new();

	Base64UrlEncode() : base(EncodedTextAsData.Default.Then().Subject.Select(WebEncoders.Base64UrlEncode)) {}
}

// TODO

public sealed class Base64UrlEncrypt : Alteration<string>
{
	public Base64UrlEncrypt(IEncryptText select) : base(select.Then().Select(Base64UrlEncode.Default)) {}
}

public sealed class UrlEncrypt : Alteration<string>
{
	public UrlEncrypt(IEncryptText select) : base(select.Then().Select(UrlEncode.Default)) {}
}