using System.Collections.Generic;

namespace DragonSpark.Application.Model.Sequences;

public readonly record struct ModelViewActionsInput<TModel, TView>
	(IEnumerable<TModel> Models, IEnumerable<TView> Views);