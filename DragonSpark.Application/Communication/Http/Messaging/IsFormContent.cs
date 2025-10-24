using System.Net.Http;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Communication.Http.Messaging;

sealed class IsFormContent : Condition<HttpRequestMessage>
{
	public static IsFormContent Default { get; } = new();

	IsFormContent()
		: base(x => x.Method == HttpMethod.Post &&
					x.Content?.Headers.ContentType?.MediaType == "application/x-www-form-urlencoded") {}
}