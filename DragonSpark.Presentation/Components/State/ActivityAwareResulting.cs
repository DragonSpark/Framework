using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{
	sealed class ActivityAwareResulting<T> : IResulting<T>
    {
	    readonly IResulting<T>           _previous;
	    readonly object                  _subject;
	    readonly IUpdateActivityReceiver _activity;

	    public ActivityAwareResulting(IResulting<T> previous, object subject)
		    : this(previous, subject, UpdateActivityReceiver.Default) {}

	    public ActivityAwareResulting(IResulting<T> previous, object subject, IUpdateActivityReceiver activity)
	    {
		    _previous = previous;
		    _subject   = subject;
		    _activity  = activity;
	    }

	    public async ValueTask<T> Get()
	    {
		    await _activity.Get((_subject, _previous));
		    try
		    {
			    return await _previous.Get();
		    }
		    finally
		    {
			    await _activity.Get(_subject);
		    }
	    }
    }
}
