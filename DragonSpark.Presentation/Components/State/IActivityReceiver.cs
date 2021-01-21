using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{
	public interface IActivityReceiver
	{
		ValueTask Start();
		ValueTask Complete();

	}
}