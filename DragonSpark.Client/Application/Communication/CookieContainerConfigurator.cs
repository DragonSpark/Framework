using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Communication
{
	public class CookieContainerConfigurator : IUnityContainerTypeConfiguration
	{
		static readonly MethodInfo RegisterMethod = typeof(CookieContainerConfigurator).GetMethod( "Register", DragonSparkBindingOptions.AllProperties );

		public void Configure( IUnityContainer container, UnityType type )
		{
			var application = container.Resolve<IApplicationContext>();
			RegisterMethod.MakeGenericMethod( ChannelType ?? type.RegistrationType ).Invoke( null, new object[] { application.Location } );
		}

		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type ChannelType { get; set; }

		static void Register<T>( Uri uri )
		{
			ClientChannelFactory<T>.Register( x =>
			{
			    var client = ClientChannelFactory<T>.DefaultFactory( x );
				client.As<IClientChannel>( y => y.GetProperty<IHttpCookieContainerManager>().NotNull( z =>
				{
					z.CookieContainer = CookieContainerFactory.Instance.Create( uri );
				} ) );
				
			    return client;
			} );
		}
	}
}