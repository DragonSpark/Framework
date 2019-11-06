using DragonSpark.Model.Selection;
using Refit;
using System.Net.Http;

namespace DragonSpark.Services.Communication
{
	public sealed class Api<T> : Select<HttpClient, T>
	{
		public static Api<T> Default { get; } = new Api<T>();

		Api() : base(RestService.For<T>) {}
	}
}