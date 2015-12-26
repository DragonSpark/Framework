using System.Threading.Tasks;

namespace DragonSpark.Modularity
{
	public interface IModuleMonitor
	{
		Task<bool> Load();

		void MarkAsLoaded( IMonitoredModule target );
	}
}