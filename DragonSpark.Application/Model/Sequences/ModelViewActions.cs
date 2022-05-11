using DragonSpark.Compose;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Linq;

namespace DragonSpark.Application.Model.Sequences;

public class ModelViewActions<TModel, TView> : IModelViewActions<TModel, TView>
	where TModel : class, IIdentityAware
	where TView : class, IIdentityAware
{
	readonly ArrayPool<TModel>                _models;
	readonly ArrayPool<TView>                 _views;
	readonly ArrayPool<Update<TModel, TView>> _update;

	protected ModelViewActions()
		: this(ArrayPool<TModel>.Shared, ArrayPool<TView>.Shared, ArrayPool<Update<TModel, TView>>.Shared) {}

	protected ModelViewActions(ArrayPool<TModel> models, ArrayPool<TView> views,
	                           ArrayPool<Update<TModel, TView>> update)
	{
		_models = models;
		_views  = views;
		_update = update;
	}

	public Actions<TModel, TView> Get(ModelViewActionsInput<TModel, TView> parameter)
	{
		var (m, v) = parameter;
		using var models = m.AsValueEnumerable().ToArray(_models);
		using var views  = v.AsValueEnumerable().ToArray(_views);

		var add = views.AsEnumerable()
		               .Outstanding(models.AsEnumerable(), x => x.Get(), x => x.Get())
		               .AsValueEnumerable()
		               .ToArray(_views)
		               .Then();
		var remove = models.AsEnumerable()
		                   .Outstanding(views.AsEnumerable(), x => x.Get(), x => x.Get())
		                   .AsValueEnumerable()
		                   .ToArray(_models)
		                   .Then();

		var update = views.Join(models, x => x.Get(), y => y.Get(), (x, y) => new Update<TModel, TView>(y, x))
		                  .AsValueEnumerable()
		                  .ToArray(_update)
		                  .Then();
		return new(add, update, remove);
	}
}