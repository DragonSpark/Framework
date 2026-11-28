using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Presentation.Components.State;

sealed class Blocker : IDepending
{
	readonly long _duration;
	long          _until;

	public Blocker(TimeSpan duration) : this((long)duration.TotalMilliseconds) {}

	public Blocker(long duration) => _duration = duration;

	public ValueTask<bool> Get(None parameter)
	{
		var now    = System.Environment.TickCount64;
		var until  = Volatile.Read(ref _until);
		var result = now >= until && Interlocked.CompareExchange(ref _until, now + _duration, until) == until;
		return result.ToOperation();
	}
}