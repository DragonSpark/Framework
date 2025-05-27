using DragonSpark.Application.Model.Values;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Security.Identity.Profile;

public class UpdateProfileState<T> : UpdateState<T> where T : ProfileBase
{
    protected UpdateProfileState(IStopAware<T> save, IDepending clear) : base(save, clear) {}
}