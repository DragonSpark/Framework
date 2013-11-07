using System.Diagnostics.Contracts;
using System.Windows.Markup;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Interceptor" )]
	public class TypeInjectionConfiguration : UnityContainerTypeConfiguration
	{
		public ITypeInterceptor Interceptor { get; set; }

		protected override void Configure(IUnityContainer container, UnityType type)
		{
			Contract.Assume( container != null );
			Contract.Assume( type != null );

			var interception = container.Configure<Interception>();
			interception.SetInterceptorFor( type.RegistrationType, type.BuildName, Interceptor ?? new VirtualMethodInterceptor() );
		}
	}
}