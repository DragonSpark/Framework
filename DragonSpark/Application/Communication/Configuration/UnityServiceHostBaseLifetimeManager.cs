using System.ServiceModel;

namespace DragonSpark.Application.Communication.Configuration
{
	/// <summary>
	/// Unity lifetime manager to support <see cref="System.ServiceModel.ServiceHostBase"/>.
	/// </summary>
	public class UnityServiceHostBaseLifetimeManager : UnityWcfLifetimeManager<ServiceHostBase>
	{
		/// <summary>
		/// Returns the appropriate extension for the current lifetime manager.
		/// </summary>
		/// <returns>The registered extension for the current lifetime manager, otherwise, null if the extension is not registered.</returns>
		protected override UnityWcfExtension<ServiceHostBase> FindExtension()
		{
			return UnityServiceHostBaseExtension.Instance;
		}
	}
}