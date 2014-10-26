using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace DragonSpark.Server.IoC
{
	public class UnityCallContextInitializer : ICallContextInitializer
	{
		public void AfterInvoke(object correlationState)
		{
			// It feels wrong going through the OperationContext to get to the channel, but since it's not passed as a parameter
			// to this method, like BeforeInvoke(), we have to do it this way.  Should we return a correlation state
			// from BeforeInvoke() to get to this?
			OperationContext.Current.Channel.Extensions.Remove( UnityContextChannelExtension.Current );
		}

		public object BeforeInvoke( InstanceContext instanceContext, IClientChannel channel, Message message )
		{
			channel.Extensions.Add( new UnityContextChannelExtension() );
			return null;
		}
	}

	public class UnityContextChannelLifetimeManager : IoCLifetimeManager<IContextChannel>
	{
		public UnityContextChannelLifetimeManager( Guid instanceKey ) : base( instanceKey )
		{}

		protected override UnityWcfExtension<IContextChannel> FindExtension()
		{
			return UnityContextChannelExtension.Current;
		}
	}

	public class UnityInstanceContextExtension : UnityWcfExtension<InstanceContext>
	{
		public static UnityInstanceContextExtension Current
		{
			get { return OperationContext.Current.InstanceContext.Extensions.Find<UnityInstanceContextExtension>(); }
		}
	}

	public class UnityInstanceContextInitializer : IInstanceContextInitializer
	{
		public void Initialize( InstanceContext instanceContext, Message message )
		{
			instanceContext.Extensions.Add( new UnityInstanceContextExtension() );

			// We need to subscribe to the Closing event so we can remove the extension.
			instanceContext.Closing += InstanceContextClosing;
		}

		static void InstanceContextClosing( object sender, EventArgs e )
		{
			sender.As<InstanceContext>( x =>
			{
				x.Closing -= InstanceContextClosing;

				// We have to get this manually, as the operation context has been disposed by now.
				var extension = x.Extensions.Find<UnityInstanceContextExtension>();
				if ( extension != null )
				{
					x.Extensions.Remove( extension );
				}
			} );
		}
	}

	public class UnityInstanceContextLifetimeManager : IoCLifetimeManager<InstanceContext>
	{
		public UnityInstanceContextLifetimeManager( Guid instanceKey ) : base( instanceKey )
		{}

		protected override UnityWcfExtension<InstanceContext> FindExtension()
		{
			return UnityInstanceContextExtension.Current;
		}
	}

	public class UnityOperationContextExtension : UnityWcfExtension<OperationContext>
	{
		public static UnityOperationContextExtension Current
		{
			get { return OperationContext.Current.Extensions.Find<UnityOperationContextExtension>(); }
		}
	}
	public class UnityOperationContextLifetimeManager : IoCLifetimeManager<OperationContext>
	{
		public UnityOperationContextLifetimeManager( Guid instanceKey ) : base( instanceKey )
		{}

		protected override UnityWcfExtension<OperationContext> FindExtension()
		{
			return UnityOperationContextExtension.Current;
		}
	}

	public class UnityOperationContextMessageInspector : IDispatchMessageInspector
	{
		public object AfterReceiveRequest( ref Message request, IClientChannel channel, InstanceContext instanceContext )
		{
			OperationContext.Current.Extensions.Add( new UnityOperationContextExtension() );
			return null;
		}

		public void BeforeSendReply( ref Message reply, object correlationState )
		{
			OperationContext.Current.Extensions.Remove( UnityOperationContextExtension.Current );
		}
	}

	public sealed class UnityServiceInstance : UnityWcfExtension<ServiceHostBase>
	{
		UnityServiceInstance()
		{}

		public static UnityServiceInstance Instance
		{
			get { return InstanceField; }
		}	static readonly UnityServiceInstance InstanceField = new UnityServiceInstance();
	}

	public class UnityWcfExtension<T> : IExtension<T>
		where T : IExtensibleObject<T>
	{
		readonly Dictionary<Guid, object> instances = new Dictionary<Guid, object>();

		public void Attach( T owner )
		{}

		public void Detach( T owner )
		{
			// If we are being detached, let's go ahead and clean up, just in case.
			var keys = new List<Guid>( instances.Keys );

			foreach ( var t in keys )
			{
				RemoveInstance( t );
			}
		}

		public void RegisterInstance( Guid key, object value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}

			instances.Add( key, value );
		}

		public object FindInstance( Guid key )
		{
			object obj;

			// We don't care whether or not this succeeds or fails.
			instances.TryGetValue( key, out obj );
			return obj;
		}

		public void RemoveInstance( Guid key )
		{
			// We don't want to use FindInstance JUST IN CASE somehow a key gets in there with a null object.

			if ( instances.ContainsKey( key ) )
			{
				// Get the instance.
				var instance = instances[ key ];

				// See if it needs disposing.
				var disposable = instance as IDisposable;
				if ( disposable != null )
				{
					disposable.Dispose();
				}

				// Remove it from the instances list.
				instances.Remove( key );
			}
		}
	}

	public class UnityServiceBehavior : IServiceBehavior
	{
		public string ContainerName { get; set; }

		public void AddBindingParameters( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters )
		{}

		public void ApplyDispatchBehavior( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{
			serviceHostBase.Extensions.Add( UnityServiceInstance.Instance );

			// We need to subscribe to the Closing event so we can remove the extension.
			serviceHostBase.Closing += ServiceHostBaseClosing;

			foreach ( var dispatcher in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>().SelectMany( channelDispatcher => channelDispatcher.Endpoints ) )
			{
				dispatcher.DispatchRuntime.MessageInspectors.Add( new UnityOperationContextMessageInspector() ); // This one.

				dispatcher.DispatchRuntime.InstanceContextInitializers.Add( new UnityInstanceContextInitializer() );
						
				foreach ( var operation in dispatcher.DispatchRuntime.Operations )
				{
					operation.CallContextInitializers.Add( new UnityCallContextInitializer() );
				}
			}
		}

		public void Validate( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{}

		static void ServiceHostBaseClosing( object sender, EventArgs e )
		{
			sender.As<ServiceHostBase>( x =>
			{
				x.Closing -= ServiceHostBaseClosing;

				// We have to get this manually, as the operation context has been disposed by now.
				x.Extensions.Find<UnityServiceInstance>().NotNull( y => x.Extensions.Remove( y ) );
			} );
		}
	}

	public class UnityContextChannelExtension : UnityWcfExtension<IContextChannel>
	{
		public static UnityContextChannelExtension Current
		{
			get { return OperationContext.Current.Channel.Extensions.Find<UnityContextChannelExtension>(); }
		}
	}

	public class ServiceBasedLifetimeManager : IoCLifetimeManager<ServiceHostBase>
	{
		public ServiceBasedLifetimeManager( Guid instanceKey ) : base( instanceKey )
		{}

		protected override UnityWcfExtension<ServiceHostBase> FindExtension()
		{
			return UnityServiceInstance.Instance;
		}
	}

	public class RequestLifetimeManager : KeyBasedLifetimeManager
	{
		public RequestLifetimeManager( string name ) : base( name )
		{}

		public override object GetValue()
		{
			var result = HttpContext.Current.Transform( x => x.Items[ Key ] );
			return result;
		}

		public override void RemoveValue() 
		{ 
			HttpContext.Current.NotNull( x => x.Items.Remove( Key ) );
		}

		public override void SetValue(object newValue) 
		{
			HttpContext.Current.NotNull( x => x.Items[ Key ] = newValue ); 
		}
	}

	public class IoCLifetimeExtensionElement : BehaviorExtensionElement
	{
		private const string ContainerConfigurationPropertyName = "containerName";

		public override Type BehaviorType
		{
			get { return typeof(UnityServiceBehavior); }
		}

		[ConfigurationProperty(ContainerConfigurationPropertyName, IsRequired = false)]
		public string ContainerName
		{
			get { return (string)base[ContainerConfigurationPropertyName]; }
			set { base[ContainerConfigurationPropertyName] = value; }
		}

		protected override object CreateBehavior()
		{
			var result = new UnityServiceBehavior { ContainerName = ContainerName };
			return result;
		}
	}

	public abstract class IoCLifetimeManager<T> : SynchronizedLifetimeManager
		where T : IExtensibleObject<T>
	{
		readonly Guid instanceKey;

		protected IoCLifetimeManager( Guid instanceKey )
		{
			this.instanceKey = instanceKey;
		}

		UnityWcfExtension<T> Extension
		{
			get
			{
				var extension = FindExtension();
				if ( extension == null )
				{
					throw new InvalidOperationException(
						string.Format( CultureInfo.CurrentCulture, "UnityWcfExtension<{0}> must be registered in WCF.", typeof(T).Name ) );
				}

				return extension;
			}
		}

		
		protected override object SynchronizedGetValue()
		{
			var result = Extension.FindInstance( instanceKey );
			return result;
		}

		protected override void SynchronizedSetValue( object newValue )
		{
			Extension.RegisterInstance( instanceKey, newValue );
		}
		
		public override void RemoveValue()
		{
			// Not called, but just in case.
			Extension.RemoveInstance( instanceKey );
		}

		protected abstract UnityWcfExtension<T> FindExtension();
	}
}
