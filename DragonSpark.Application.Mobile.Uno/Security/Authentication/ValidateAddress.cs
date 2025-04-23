using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Mobile.Uno.Security.Authentication;

public class ValidateAddress : ISelecting<ValidateAddressInput, Uri?>
{
    readonly IToken<string>                    _receive;
    readonly ISelect<ResumeSessionInput, Uri?> _address;

    public ValidateAddress(IToken<string> receive) : this(receive, ResumeSession.Default) {}

    public ValidateAddress(IToken<string> receive, ISelect<ResumeSessionInput, Uri?> address)
    {
        _receive = receive;
        _address = address;
    }

    public async ValueTask<Uri?> Get(ValidateAddressInput parameter)
    {
        var (identifier, cancellationToken) = parameter;
        var content = await _receive.Off(cancellationToken);
        var result  = _address.Get(new(identifier, new(content)));
        return result;
    }
}