using DragonSpark.Application.Runtime.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public interface IActivityReceiver
{
	ValueTask Start(ActivityReceiverInput input);
	ValueTask Complete();

	bool Active { get; }
}
// TODO
public readonly record struct ActivityReceiverInput(string? Message = null, ITokenHandle? Handle = null)
{
	public static ActivityReceiverInput Default { get; } = new();
}