using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity.Client;

sealed class ClientKeyHashProcessStore : Variable<string>
{
    public static ClientKeyHashProcessStore Default { get; } = new();

    ClientKeyHashProcessStore() {}
}