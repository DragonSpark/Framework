using System;

namespace DragonSpark.IoC
{
	/// <summary>
	/// Represents a PerRequest registration.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
	public sealed class PerRequestAttribute : ComponentRegistrationBaseAttribute
	{
		/// <summary>
		/// Gets or sets the implementation.
		/// </summary>
		/// <value>The implementation.</value>
		public Type Implementation { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PerRequestAttribute"/> class.
		/// </summary>
		public PerRequestAttribute() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="PerRequestAttribute"/> class.
		/// </summary>
		/// <param name="service">The type key.</param>
		public PerRequestAttribute(Type service)
		{
			Service = service;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PerRequestAttribute"/> class.
		/// </summary>
		/// <param name="name">The key.</param>
		/// <param name="service">The type name.</param>
		public PerRequestAttribute(string name, Type service)
		{
			Name = name;
			Service = service;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PerRequestAttribute"/> class.
		/// </summary>
		/// <param name="name">The key.</param>
		public PerRequestAttribute(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Gets the component info.
		/// </summary>
		/// <param name="decoratedType">Type of the decorated.</param>
		/// <returns></returns>
		public override IComponentRegistration GetComponentInfo(Type decoratedType)
		{
			Implementation = decoratedType;
			if (Service == null) Service = decoratedType;
			return this;
		}
	}
}