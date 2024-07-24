using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public interface IActivityReceiver
{
	ValueTask Start(ActivityReceiverInput input);
	ValueTask Complete();

	bool Active { get; }
}