using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly record struct ActionMemory<TView, TModel>(Memory<TView> Add, Memory<Update<TView, TModel>> Update,
                                                          Memory<TModel> Delete);