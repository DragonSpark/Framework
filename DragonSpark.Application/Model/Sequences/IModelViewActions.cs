using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Model.Sequences;

public interface IModelViewActions<TModel, TView>
	: ISelect<ViewToModelInput<TModel, TView>, Actions<TModel, TView>> {}