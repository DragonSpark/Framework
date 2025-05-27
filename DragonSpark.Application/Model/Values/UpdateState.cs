using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Model.Values;

public class UpdateState<T> : IDepending<T?>
{
    readonly IStopAware<T> _save;
    readonly IDepending    _clear;

    protected UpdateState(IStopAware<T> save, IDepending clear)
    {
        _save  = save;
        _clear = clear;
    }

    public async ValueTask<bool> Get(Stop<T?> parameter)
    {
        var (subject, stop) = parameter;
        if (subject is not null)
        {
            await _save.Off(new(subject, stop));
            return true;
        }

        return await _clear.Off(new(None.Default, stop));
    }
}