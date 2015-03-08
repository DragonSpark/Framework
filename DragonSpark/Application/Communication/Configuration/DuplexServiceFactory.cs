using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Communication.Configuration
{
	[ContentProperty( "CallbackBuildKey" )]
	public class DuplexServiceFactory : GenericItemFactoryBase
	{
		public NamedTypeBuildKey CallbackBuildKey { get; set; }

		protected override TItem Create<TItem>( IUnityContainer container, string buildName )
		{
			Contract.Assume( container != null );

			var callback = CallbackBuildKey.Create( container );
			var factory = new DuplexChannelFactory<TItem>( callback, buildName ?? "*" );
			var result = factory.CreateChannel();
			result.As<IClientChannel>( x =>
			{
				container.RegisterDisposable( x );
				x.Closed += ( sender, args ) => factory.TryDispose();
			} );
			return result;
		}
	}
}