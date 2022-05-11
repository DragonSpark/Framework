using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Model.Sequences;

public interface IModelViewActions<TModel, TView>
	: ISelect<ModelViewActionsInput<TModel, TView>, Actions<TModel, TView>> {}