using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class IsUserRequest : InverseCondition<HttpRequest>
{
	public static IsUserRequest Default { get; } = new();

	IsUserRequest() : base(IsBotRequest.Default) {}
}