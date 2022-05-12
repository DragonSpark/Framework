using DragonSpark.Compose;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Linq;

namespace DragonSpark.Application.Model.Sequences;

public class ViewToModel<TView, TModel> : ViewToModel<Guid, TView, TModel>
	where TView : class, IIdentityAware
	where TModel : class, IIdentityAware
{
	protected ViewToModel() : this(x => x.Get(), x => x.Get(), _ => true) {}

	protected ViewToModel(Func<TView, Guid> view, Func<TModel, Guid> model, Func<Update<TView, TModel>, bool> update)
		: base(view, model, update) {}
}

public class ViewToModel<TKey, TView, TModel> : IModelViewActions<TView, TModel>

{
	readonly static ArrayPool<TModel>                Models = ArrayPool<TModel>.Shared;
	readonly static ArrayPool<TView>                 Views  = ArrayPool<TView>.Shared;
	readonly static ArrayPool<Update<TView, TModel>> Update = ArrayPool<Update<TView, TModel>>.Shared;

	readonly Func<TView, TKey>                 _view;
	readonly Func<TModel, TKey>                _model;
	readonly Func<Update<TView, TModel>, bool> _update;

	protected ViewToModel(Func<TView, TKey> view, Func<TModel, TKey> model) : this(view, model, _ => true) {}

	protected ViewToModel(Func<TView, TKey> view, Func<TModel, TKey> model, Func<Update<TView, TModel>, bool> update)
	{
		_view   = view;
		_model  = model;
		_update = update;
	}

	public Actions<TView, TModel> Get(ViewToModelInput<TView, TModel> parameter)
	{
		var (v, m) = parameter;
		using var views  = v.AsValueEnumerable().ToArray(Views);
		using var models = m.AsValueEnumerable().ToArray(Models);

		var add    = views.Outstanding(models.AsEnumerable(), _view, _model).AsValueEnumerable().ToArray(Views);
		var remove = models.Outstanding(views, _model, _view).AsValueEnumerable().ToArray(Models);
		var update = views.Join(models, _view, _model, (x, y) => new Update<TView, TModel>(x, y))
		                  .AsValueEnumerable()
		                  .Where(_update)
		                  .ToArray(Update);
		return new(add, update, remove);
	}
}