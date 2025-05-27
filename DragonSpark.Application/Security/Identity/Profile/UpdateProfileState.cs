using DragonSpark.Application.Model.Values;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Security.Identity.Profile;

public class UpdateProfileState<T> : SaveState<T> where T : ProfileBase
{
    protected UpdateProfileState(IMutable<T?> store, IStopAware<T> value) : base(store, value) {}
}