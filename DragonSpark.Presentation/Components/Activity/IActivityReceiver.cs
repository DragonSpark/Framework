using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Activity
{
	public interface IActivityReceiver
	{
		ValueTask Start();
		ValueTask Complete();

	}
}