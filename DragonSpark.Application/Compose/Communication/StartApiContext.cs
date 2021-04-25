using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace DragonSpark.Application.Compose.Communication
{
	public sealed class StartApiContext<T> where T : class
	{
		readonly IServiceCollection _subject;

		public StartApiContext(IServiceCollection subject) => _subject = subject;

		public RegisterApiContext<T> UsingConfiguration() 
			=> Using(x => x.GetRequiredService<RefitConfiguration>().Get());

		public RegisterApiContext<T> Using(Func<IServiceProvider, RefitSettings?> settings) => new(_subject, settings);
	}
}