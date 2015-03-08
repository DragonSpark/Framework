using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Presentation.Entity
{
	interface ILoadOperationListener
	{
		void OnLoad( LoadOperation operation );
	}
}