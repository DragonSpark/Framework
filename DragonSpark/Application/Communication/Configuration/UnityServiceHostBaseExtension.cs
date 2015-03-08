using System.ServiceModel;

namespace DragonSpark.Application.Communication.Configuration
{
	public sealed class UnityServiceHostBaseExtension : UnityWcfExtension<ServiceHostBase>
	{
		UnityServiceHostBaseExtension()
		{}

		public static UnityServiceHostBaseExtension Instance
		{
			get { return InstanceField; }
		}	static readonly UnityServiceHostBaseExtension InstanceField = new UnityServiceHostBaseExtension();

		/*/// <summary>
		/// Gets the <see cref="UnityServiceHostBaseExtension"/> for the current service host.
		/// </summary>
		public static UnityServiceHostBaseExtension Current
		{
			get
			{
				
				var operationContext = OperationContext.Current;
				var serviceHostBase = operationContext.Host;
				var unityServiceHostBaseExtension = serviceHostBase.Extensions.Find<UnityServiceHostBaseExtension>();
				return unityServiceHostBaseExtension;
			}
		}*/
	}
}