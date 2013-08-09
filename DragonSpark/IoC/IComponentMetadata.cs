using System;

namespace DragonSpark.IoC
{
	/// <summary>
	/// Metadat the describes a component registration.
	/// </summary>
	public interface IComponentMetadata
	{
		Priority Priority { get; }

		/// <summary>
		/// Gets the component info.
		/// </summary>
		/// <param name="decoratedType">Type of the decorated.</param>
		/// <returns></returns>
		IComponentRegistration GetComponentInfo(Type decoratedType);
	}
}