using DragonSpark.Model.Results;
using Refit;

namespace DragonSpark.Application.Compose.Communication
{
	sealed class RefitConfiguration : IResult<RefitSettings>
	{
		readonly IHttpContentSerializer _serializer;

		public RefitConfiguration(IHttpContentSerializer serializer) => _serializer = serializer;

		public RefitSettings Get() => new(_serializer);
	}
}