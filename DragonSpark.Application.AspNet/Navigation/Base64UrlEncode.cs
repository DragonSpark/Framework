using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace DragonSpark.Application.AspNet.Navigation;

public sealed class Base64UrlEncode : Alteration<string>
{
	public static Base64UrlEncode Default { get; } = new();

	Base64UrlEncode() : base(EncodedTextAsData.Default.Then().Subject.Select(WebEncoders.Base64UrlEncode)) {}
}