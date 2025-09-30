using System.Text;
using DragonSpark.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace DragonSpark.Application.AspNet.Navigation;

public sealed class Base64UrlDecode : Base64DecodeBase
{
	public static Base64UrlDecode Default { get; } = new();

	Base64UrlDecode() : this(Encoding.UTF8) {}

	public Base64UrlDecode(Encoding encoding) : base(WebEncoders.Base64UrlDecode, encoding) {}
}