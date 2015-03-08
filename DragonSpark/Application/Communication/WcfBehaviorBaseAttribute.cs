using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace DragonSpark.Application.Communication
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
	public abstract class WcfBehaviorBaseAttribute : Attribute, IServiceBehavior
	{
		readonly Type _behaviorType;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="typeBehavior">IDispatchMessageInspector, IErrorHandler of IParameterInspector</param>
		protected WcfBehaviorBaseAttribute( Type typeBehavior )
		{
			_behaviorType = typeBehavior;
		}

		void IServiceBehavior.AddBindingParameters( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters )
		{
			AddBindingParameters( serviceDescription, serviceHostBase, endpoints, bindingParameters );
		}

		protected virtual void AddBindingParameters( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters )
		{}

		void IServiceBehavior.ApplyDispatchBehavior( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{
			ApplyDispatchBehavior( serviceHostBase );
		}

		protected virtual void ApplyDispatchBehavior( ServiceHostBase serviceHostBase )
		{
			object behavior;
			try
			{
				behavior = Activator.CreateInstance( _behaviorType );
			}
			catch ( MissingMethodException e )
			{
				throw new ArgumentException( "The Type specified in the BehaviorAttribute constructor must have a public empty constructor.", e );
			}
			catch ( InvalidCastException e )
			{
				throw new ArgumentException( "The Type specified in the BehaviorAttribute constructor must implement IDispatchMessageInspector, IParamaterInspector of IErrorHandler", e );
			}

			foreach ( ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers )
			{
				if ( behavior is IParameterInspector )
				{
					foreach ( var op in channelDispatcher.Endpoints.SelectMany( epDisp => epDisp.DispatchRuntime.Operations ) )
					{
						op.ParameterInspectors.Add( (IParameterInspector)behavior );
					}
				}
				else if ( behavior is IErrorHandler )
				{
					channelDispatcher.ErrorHandlers.Add( (IErrorHandler)behavior );
				}
				else if ( behavior is IDispatchMessageInspector )
				{
					foreach ( var endpointDispatcher in channelDispatcher.Endpoints )
					{
						endpointDispatcher.DispatchRuntime.MessageInspectors.Add( (IDispatchMessageInspector)behavior );
					}
				}
			}
		}

		void IServiceBehavior.Validate( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{
			Validate( serviceDescription, serviceHostBase );
		}

		protected virtual void Validate( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{}
	}
}
