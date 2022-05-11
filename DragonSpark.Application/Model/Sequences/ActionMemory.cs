using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly record struct ActionMemory<TModel, TView>(Memory<TView> Add, Memory<Update<TModel, TView>> Update,
                                                          Memory<TModel> Delete);