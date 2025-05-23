using System.Threading.Tasks;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Communication.Http.Security;

public class PersistAccessTokenView : IStopAware<PersistAccessTokenViewInput>
{
    readonly IStorageValue<AccessTokenView> _value;

    protected PersistAccessTokenView(IStorageValue<AccessTokenView> value) => _value = value;

    public ValueTask Get(Stop<PersistAccessTokenViewInput> parameter)
    {
        var ((identifier, response), stop) = parameter;
        return _value.Get(new(new(identifier, response), stop));
    }
}