using System.Linq;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class AutoRegisterInterfaces : UnityContainerTypeConfiguration
	{
		public bool ForceRegistration { get; set; }

		protected override void Configure(IUnityContainer container, UnityType type)
		{
			var query = type.Transform( x => x.RegistrationType.ResolveInterfaces().Where( y => ForceRegistration || !container.IsRegisteredOrMapped( y ) ) );
			query.Apply( item => container.RegisterType( item, type.MapTo ?? type.RegistrationType ) );
		}
	}
}