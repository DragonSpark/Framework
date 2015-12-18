using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Runtime.Values
{
	class AmbientKeyLocator : IAmbientKeyLocator
	{
		public static AmbientKeyLocator Instance { get; } = new AmbientKeyLocator();

		public IAmbientKey Locate( IServiceLocator context )
		{
			var result = context.GetInstance<IAmbientKey>();
			return result;
		}
	}
}