using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace DragonSpark.Application.Communication
{
	public class CyclicReferencesAwareContractBehavior : IContractBehavior
	{
		const Int32 MaxItemsInObjectGraph = 0xFFFF;
		const bool IgnoreExtensionDataObject = false;

		readonly bool _on;

		public CyclicReferencesAwareContractBehavior( bool on )
		{
			_on = on;
		}

		#region IContractBehavior Members
		public void AddBindingParameters( ContractDescription contractDescription, ServiceEndpoint endpoint,
		                                  BindingParameterCollection bindingParameters )
		{}

		public void ApplyClientBehavior( ContractDescription contractDescription, ServiceEndpoint endpoint,
		                                 ClientRuntime clientRuntime )
		{
			ReplaceDataContractSerializerOperationBehaviors( contractDescription, _on );
		}

		public void ApplyDispatchBehavior( ContractDescription contractDescription, ServiceEndpoint endpoint,
		                                   DispatchRuntime dispatchRuntime )
		{
			ReplaceDataContractSerializerOperationBehaviors( contractDescription, _on );
		}

		internal static void ReplaceDataContractSerializerOperationBehaviors( ContractDescription contractDescription, bool on )
		{
			foreach ( var operation in contractDescription.Operations )
			{
				ReplaceDataContractSerializerOperationBehavior( operation, on );
			}
		}

		internal static void ReplaceDataContractSerializerOperationBehavior( OperationDescription operation, bool on )
		{
			if ( operation.Behaviors.Remove( typeof(DataContractSerializerOperationBehavior) ) ||
			     operation.Behaviors.Remove( typeof(ApplyCyclicDataContractSerializerOperationBehavior) ) )
			{
				operation.Behaviors.Add( new ApplyCyclicDataContractSerializerOperationBehavior( operation, MaxItemsInObjectGraph,
				                                                                                 IgnoreExtensionDataObject, on ) );
			}
		}

		public void Validate( ContractDescription contractDescription, ServiceEndpoint endpoint )
		{}
		#endregion
	}
}