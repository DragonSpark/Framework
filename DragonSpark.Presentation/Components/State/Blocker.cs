using System;
using System.Threading.Tasks;
using System.Timers;
using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Components.State;

sealed class Blocker : IDepending
{
    readonly Switch    _active;
    readonly AsyncLock _lock;
    readonly Timer     _timer;

    public Blocker(TimeSpan duration)
        : this(new(), new(), new() { AutoReset = false, Enabled = false, Interval = duration.TotalMilliseconds }) {}

    public Blocker(Switch active, AsyncLock @lock, Timer timer)
    {
        _active        =  active;
        _lock          =  @lock;
        _timer         =  timer;
        _timer.Elapsed += (_, _) => _active.Down();
    }

    public async ValueTask<bool> Get(None parameter)
    {
        using var _      = await _lock.LockAsync().On();
        var       result = _active.Up();
        if (result)
        {
            _timer.Stop();
            _timer.Start();
        }

        return result;
    }
}