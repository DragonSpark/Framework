using DragonSpark.Application.Model.Values;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Security.Identity.Profile;

public class ClearProfileState<T> : ClearState<T> where T : ProfileBase
{
    protected ClearProfileState(IMutable<T?> store, IDepending storage) : base(store, storage) {}
}