using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Timers;

namespace DragonSpark.Application.Runtime;

sealed class CreateTimer<T> : ISelect<T, Timer>
{
    readonly IDictionary<T, Timer> _store;
    readonly Operate<T> _subject;
    readonly double _interval;

    public CreateTimer(IDictionary<T, Timer> store, Operate<T> subject, double interval)
    {
        _store = store;
        _subject = subject;
        _interval = interval;
    }

    [MustDisposeResource]
    public Timer Get(T parameter)
    {
        var result = new Timer { Enabled = false, AutoReset = false, Interval = _interval };
        // Who am I to argue: https://stackoverflow.com/questions/38917818/pass-async-callback-to-timer-constructor#comment91001639_38918443
        result.Elapsed += async (_, _) =>
                          {
                              if (_store.Remove(parameter))
                              {
	                              await _subject(parameter).Await();
                              }
                          };
        return result;
    }
}
