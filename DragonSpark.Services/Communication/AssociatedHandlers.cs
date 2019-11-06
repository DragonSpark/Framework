using DragonSpark.Model.Selection.Stores;
using System.Net.Http;

namespace DragonSpark.Services.Communication
{
	sealed class AssociatedHandlers : ReferenceValueTable<HttpClient, System.Net.Http.HttpClientHandler>
	{
		public static AssociatedHandlers Default { get; } = new AssociatedHandlers();

		AssociatedHandlers() {}
	}
}