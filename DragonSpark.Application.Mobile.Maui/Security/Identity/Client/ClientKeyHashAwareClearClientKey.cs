using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity.Client;

sealed class ClientKeyHashAwareClearClientKey : IClearClientKey
{
    readonly IClearClientKey   _previous;
    readonly IMutable<string?> _store;

    public ClientKeyHashAwareClearClientKey(IClearClientKey previous) 
        : this(previous, ClientKeyHashProcessStore.Default) {}

    public ClientKeyHashAwareClearClientKey(IClearClientKey previous, IMutable<string?> store)
    {
        _previous = previous;
        _store    = store;
    }

    public ValueTask<bool> Get(Stop<None> parameter)
    {
        _store.Execute(null);
        return _previous.Get(parameter);
    }
}