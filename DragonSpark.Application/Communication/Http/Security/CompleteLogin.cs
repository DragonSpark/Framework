using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Communication.Http.Security;

sealed class CompleteLogin : ICompleteLogin
{
    readonly IUpdateTokenState _save;

    public CompleteLogin(IUpdateTokenState save) => _save = save;

    public async ValueTask Get(Stop<AccessTokenView?> parameter)
    {
        var (subject, stop) = parameter;
        await _save.Off(new(subject, stop));
    }
}