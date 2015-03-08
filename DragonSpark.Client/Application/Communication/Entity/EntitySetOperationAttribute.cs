using System;
using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Communication.Entity
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class EntitySetOperationAttribute : Attribute
	{
		public EntitySetOperations Operations { get; set; }

		public string AuthorizedRoles { get; set; }
	}
}