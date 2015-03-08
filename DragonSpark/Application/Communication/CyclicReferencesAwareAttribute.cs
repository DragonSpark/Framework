using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace DragonSpark.Application.Communication
{
	[AttributeUsage( AttributeTargets.Interface | AttributeTargets.Method )]
	public sealed  class CyclicReferencesAwareAttribute : Attribute, IContractBehavior, IOperationBehavior
	{
		readonly bool _on = true;

		public CyclicReferencesAwareAttribute( bool on )
		{
			_on = on;
		}

		public bool On
		{
			get { return ( _on ); }
		}

		#region IOperationBehavior Members
		void IOperationBehavior.AddBindingParameters( OperationDescription operationDescription,
		                                              BindingParameterCollection bindingParameters )
		{}

		void IOperationBehavior.ApplyClientBehavior( OperationDescription operationDescription,
		                                             ClientOperation clientOperation )
		{
			CyclicReferencesAwareContractBehavior.ReplaceDataContractSerializerOperationBehavior( operationDescription, On );
		}

		void IOperationBehavior.ApplyDispatchBehavior( OperationDescription operationDescription,
		                                               DispatchOperation dispatchOperation )
		{
			CyclicReferencesAwareContractBehavior.ReplaceDataContractSerializerOperationBehavior( operationDescription, On );
		}

		void IOperationBehavior.Validate( OperationDescription operationDescription )
		{}
		#endregion

		#region IContractBehavior Members
		void IContractBehavior.AddBindingParameters( ContractDescription contractDescription, ServiceEndpoint endpoint,
		                                             BindingParameterCollection bindingParameters )
		{}

		void IContractBehavior.ApplyClientBehavior( ContractDescription contractDescription, ServiceEndpoint endpoint,
		                                            ClientRuntime clientRuntime )
		{
			CyclicReferencesAwareContractBehavior.ReplaceDataContractSerializerOperationBehaviors( contractDescription, On );
		}

		void IContractBehavior.ApplyDispatchBehavior( ContractDescription contractDescription, ServiceEndpoint endpoint,
		                                              DispatchRuntime dispatchRuntime )
		{
			CyclicReferencesAwareContractBehavior.ReplaceDataContractSerializerOperationBehaviors( contractDescription, On );
		}

		void IContractBehavior.Validate( ContractDescription contractDescription, ServiceEndpoint endpoint )
		{}
		#endregion
	}
}