using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Runtime.Values
{
	public interface IAmbientKeyLocator
	{
		IAmbientKey Locate( IServiceLocator context );
	}
}