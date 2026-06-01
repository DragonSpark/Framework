using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Sequences;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Environment.Browser;

public interface ICreateReference<T> : IStopAware<CreateReferenceInput<T>, IJSObjectReference> where T : IArray<object>;