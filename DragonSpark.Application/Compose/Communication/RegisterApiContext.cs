using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Net.Http;

namespace DragonSpark.Application.Compose.Communication
{
	public sealed class RegisterApiContext<T> where T : class
	{
		readonly IServiceCollection _subject;
		readonly RefitSettings?     _settings;

		public RegisterApiContext(IServiceCollection subject, RefitSettings? settings = null)
		{
			_subject       = subject;
			_settings = settings;
		}

		public RegistrationResult Configure<TConfiguration>(Func<TConfiguration, Uri> baseUri)
			where TConfiguration : class
			=> Configure(new Configure<TConfiguration>(baseUri).Execute);

		public RegistrationResult Configure(Action<IServiceProvider, HttpClient> configure)
			=> new ConfigureApi<T>(configure, _settings).Parameter(_subject).To(x => new RegistrationResult(x));
	}
}
