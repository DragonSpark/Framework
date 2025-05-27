using DragonSpark.Application.Model.Values;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Communication.Http.Security;

public class UpdateTokenState : UpdateState<AccessTokenView>, IUpdateTokenState
{
    protected UpdateTokenState(ISaveTokenState save, IDepending clear) : base(save, clear) {}
}