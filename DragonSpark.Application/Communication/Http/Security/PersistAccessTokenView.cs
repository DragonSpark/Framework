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
        var ((identifier, response), _) = parameter;
        return _value.Get(new(identifier, response));
    }
}