using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Communication.Entity
{
	public interface IEntitySetOperationProfile : IAuthorizable
	{
		EntitySetOperations Operations { get; }
	}
}