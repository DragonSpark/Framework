using DragonSpark.Application.Model.Values;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http.Security;

public class ClearTokenState : ClearState<AccessTokenView>
{
    protected ClearTokenState(IMutable<AccessTokenView?> store, IDepending storage) : base(store, storage) {}
}