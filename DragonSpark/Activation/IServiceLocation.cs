using DragonSpark.Runtime.Values;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Activation
{
	public interface IServiceLocation : IWritableValue<IServiceLocator>
	{
		bool IsAvailable { get; }
	}
}