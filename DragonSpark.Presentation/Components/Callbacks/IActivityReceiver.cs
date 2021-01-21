using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Callbacks
{
	public interface IActivityReceiver
	{
		ValueTask Start();
		ValueTask Complete();

	}
}