using DragonSpark.Model.Selection.Alterations;
using System.Net;

namespace DragonSpark.Application.Navigation;

public sealed class UrlDecode : Alteration<string>
{
	public static UrlDecode Default { get; } = new();

	UrlDecode() : base(WebUtility.UrlDecode) {}
}