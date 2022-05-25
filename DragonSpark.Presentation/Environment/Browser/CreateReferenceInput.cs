using DragonSpark.Model.Sequences;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Environment.Browser;

public readonly record struct CreateReferenceInput<T>(IJSObjectReference Reference, T Input) where T : IArray<object>;