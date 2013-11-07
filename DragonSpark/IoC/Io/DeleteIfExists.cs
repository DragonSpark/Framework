using System.IO;
using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Io
{
	public class DeleteIfExists : UnityContainerTypeConfiguration
	{
		protected override void Configure(IUnityContainer container, UnityType type)
		{
			container.Resolve<FileInfo>( type.BuildName ).NotNull( x => x.Exists.IsTrue( () => Logging.Log.Try( x.Delete ) ) );
			base.Configure(container, type);
		}
	}
}