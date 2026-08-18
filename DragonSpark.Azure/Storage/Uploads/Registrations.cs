using DragonSpark.Application.AspNet.Runtime;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Azure.Storage.Uploads;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<FileStorageSettings>()
		         .Start<IUploadRoot>()
		         .Forward<UploadRoot>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         .Then.Start<UploadsControllerBase.Error>().Singleton()
		         //
		         .Then.Start<ITemporaryPath>()
		         .Forward<TemporaryPath>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
			;
	}
}