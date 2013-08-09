using System;

namespace DragonSpark.IoC
{
	/// <summary>
	/// Represents the registration of an existing instance.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class InstanceAttribute : ComponentRegistrationBaseAttribute
	{
		/// <summary>
		/// Gets or sets the implementation.
		/// </summary>
		/// <value>The implementation.</value>
		public object Implementation { get; set; }

		/// <summary>
		/// Gets the component info.
		/// </summary>
		/// <param name="decoratedType">Type of the decorated.</param>
		/// <returns></returns>
		public override IComponentRegistration GetComponentInfo(Type decoratedType)
		{
			throw new NotSupportedException();
		}
	}
}