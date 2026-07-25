using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class ComposeEntityTypes : Instance<IModel>, IEntityTypes
{
	readonly IModel                                   _model;
	readonly IReadOnlyDictionary<string, IEntityType> _named;
	readonly IReadOnlyDictionary<Type, Type>          _forwarded;

	public ComposeEntityTypes(IModel model, IReadOnlyDictionary<Type, Type> forwarded)
		: this(model, model.GetEntityTypes().ToDictionary(t => t.Name), forwarded) {}

	public ComposeEntityTypes(IModel model, IReadOnlyDictionary<string, IEntityType> named,
	                          IReadOnlyDictionary<Type, Type> forwarded) : base(model)
	{
		_forwarded = forwarded;
		_named     = named;
		_model     = model;
	}

	public IEntityType? Get(IEntityType parameter) => _named.TryGetValue(parameter.Name, out var to)
		                                                  ? to
		                                                  : _forwarded.TryGetValue(parameter.ClrType, out var forwarded)
			                                                  ? _model.FindEntityType(forwarded)
			                                                  : null;
}