using System.Collections.Generic;

namespace DragonSpark.Application.Model.Sequences;

public readonly record struct ViewToModelInput<TView, TModel>(IEnumerable<TView> Views, IEnumerable<TModel> Models);