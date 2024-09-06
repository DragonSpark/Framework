using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public interface IActivityReceiver
{
	ValueTask Start(ActivityOptions input);
	ValueTask Complete(bool refresh = false);

	bool Active { get; }
}