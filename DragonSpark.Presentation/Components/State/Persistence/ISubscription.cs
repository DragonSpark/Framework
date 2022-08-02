using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.State.Persistence;

public interface ISubscription<T> : ISelect<T, PersistingComponentStateSubscription>, IResult<Pop<T>> {}