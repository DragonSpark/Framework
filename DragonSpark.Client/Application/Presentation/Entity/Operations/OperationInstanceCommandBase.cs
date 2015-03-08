using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
	public class OperationInstanceCommandBase : OperationInstanceCommandBase<OperationBase>
	{
		public OperationInstanceCommandBase( OperationBase operation ) : base( operation )
		{}
	}

	public abstract class OperationInstanceCommandBase<TOperation> : EntityOperationCommmandBase where TOperation : OperationBase
	{
		readonly TOperation operation;

		protected OperationInstanceCommandBase( TOperation operation )
		{
			this.operation = operation;
		}

		protected TOperation Operation
		{
			get { return operation; }
		}

		protected override OperationBase ResolveOperation()
		{
			return operation;
		}
	}
}