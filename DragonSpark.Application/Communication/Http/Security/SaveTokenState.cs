using DragonSpark.Application.Model.Values;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http.Security;

public class SaveTokenState : SaveState<AccessTokenView>, ISaveTokenState
{
    protected SaveTokenState(IMutable<AccessTokenView?> store, IStorageValue<AccessTokenView> value) 
        : base(store, value) {}
}