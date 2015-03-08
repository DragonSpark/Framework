using System.ServiceModel.DomainServices.Client;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
    public class SubmitSourceOperation : SubmitSourceOperation<DomainCollectionViewSource>
	{}

	public class SubmitSourceOperation<TSource> : EntityOperationCommmandBase<TSource> where TSource : DomainCollectionViewSource
	{
		protected override OperationBase ResolveOperation()
		{
			var result = ContextChecked.DomainContext.SubmitChanges();
			return result;
		}

		[DefaultPropertyValue( "Submitting Changes to Service" )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}
	}
}