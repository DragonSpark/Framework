using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace DragonSpark.Application.Compose.Communication;

public static class Extensions
{
	public static ConfiguredApiContextRegistration<T> WithState<T>(this ConfiguredApiContextRegistration<T> @this)
		where T : class
		=> @this.Append(ApplyState.Default.Execute);

	public static IServiceCollection AddRefit<T>(this IServiceCollection @this)
		where T : class, IResult<IHttpContentSerializer>
		=> Communication.AddRefit<T>.Default.Parameter(@this);
}