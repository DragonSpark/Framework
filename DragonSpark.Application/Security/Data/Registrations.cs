using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;

namespace DragonSpark.Application.Security.Data;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<EncryptionSettings>()
		         //
		         .Start<X509Certificate2>()
		         .Use<EncryptionCertificate>()
		         .Singleton()
		         
			;
	}
}