using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose
{
	public interface IServiceTypes : ISelect<IServiceCollection, IRelatedTypes> {}
}