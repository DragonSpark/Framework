using DragonSpark.Model.Selection.Alterations;
using System.Net;

namespace DragonSpark.Application.Navigation;

public sealed class UrlEncode : Alteration<string>
{
	public static UrlEncode Default { get; } = new();

	UrlEncode() : base(WebUtility.UrlEncode) {}
}