using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class AppendedOperate : IOperation
{
    readonly Operate _previous, _current;

    public AppendedOperate(Operate previous, Operate current)
    {
        _previous = previous;
        _current = current;
    }

    public async ValueTask Get()
    {
        await _previous().Await();
        await _current().Await();
    }
}
