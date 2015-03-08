using System.Diagnostics.Contracts;
using System.Windows.Markup;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using DragonSpark.Extensions;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Interceptor" )]
	public class InstanceInjectionConfiguration : UnityContainerTypeConfiguration
	{
		public IInstanceInterceptor Interceptor { get; set; }

		protected override void Configure(IUnityContainer container, UnityType type)
		{
			Contract.Assume( container != null );
			Contract.Assume( type != null );

			var interception = container.Configure<Interception>();
			interception.SetInterceptorFor( type.RegistrationType, type.BuildName, Interceptor ?? new InterfaceInterceptor() );
		}
	}
}