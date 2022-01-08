using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose;

sealed class ServiceTypes : FixedResult<IServiceCollection, IRelatedTypes>, IServiceTypes
{
	public static ServiceTypes Default { get; } = new();

	ServiceTypes() : this(RelatedTypes.Default) {}

	public ServiceTypes(IRelatedTypes instance) : base(instance) {}
}